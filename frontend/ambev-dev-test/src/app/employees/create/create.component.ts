import { Component, inject, OnInit, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PasswordStrengthMeterComponent } from 'angular-password-strength-meter';
import { NgxMaskDirective } from 'ngx-mask';
import { MustMatchDirective } from 'src/app/core/directives/mustmatch.directive';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeService } from 'src/app/core/services/employee.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-employees-create-page',
  standalone: true,
  imports: [SharedModule, RouterModule, NgxMaskDirective, MustMatchDirective, PasswordStrengthMeterComponent],
  providers: [],
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss'
})
export default class EmployeeCreatePageComponent implements OnInit {
  private readonly formData = viewChild<NgForm>('formData');
  private readonly employeeService = inject(EmployeeService);
  private readonly authService = inject(AuthService);

  employee = {
    firstName: '',
    lastName: '',
    email: '',
    document: '',
    birthDate: '',
    superior: '',
    password: '',
    passwordConfirm: '',
    role: {
      value: '',
      toNumber: () => this.parseInt(this.employee.role.value),
      invalid: () => (this.employee.role || "0").toNumber() > this.currentUserRole
    }
  };

  superiors: any = [];
  phones: any = [];
  phonePrefix = '';
  phoneNumber = '';
  phoneType = '';
  passwordStrengthPattern = /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/;
  passwordScore = 0;
  currentUserRole = 0;

  async ngOnInit() {
    const user = this.authService.getUserData();
    const superiors = await this.employeeService.getAll();

    //Remove myself from the superior's list
    this.superiors = superiors.filter((sup: any) => sup.id !== user.id);

    this.currentUserRole = this.getRoleValue(user.role);
  }

  //prettier-ignore
  getPhoneType (type: string) {
    return type === "1"
      ? "Home"
      : type === "2"
        ? "Cellular"
        : "Work"
  }

  //prettier-ignore
  getRoleValue(role: string) {
    switch (role) {
      case "Admin": return 600;
      case "President": return 500;
      case "Director": return 400;
      case "Manager": return 300;
      case "Coordinator": return 200;
      case "DeveloperDeveloper": return 100;
      default: return 0;
    }
  }

  parseInt(val: string) {
    return val ? Number(val) : 0;
  }

  addPhone() {
    if (this.phonePrefix.trim() !== '' && this.phoneNumber.trim() !== '' && this.phoneType.trim() !== '') {
      this.phones.push({
        prefix: this.phonePrefix,
        number: this.phoneNumber,
        type: this.phoneType
      });
      this.clearPhoneForm();
    }
  }

  clearPhoneForm() {
    this.phonePrefix = '';
    this.phoneNumber = '';
    this.phoneType = '';
  }

  onPasswordStrengthChange(score: number | null) {
    this.passwordScore = score || 0;
  }

  create() {
    console.log(this.formData()?.valid);
  }
}
