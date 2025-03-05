import { Component, inject, OnInit, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeService } from 'src/app/core/services/employee.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-employees-update-page',
  standalone: true,
  imports: [SharedModule, RouterModule, NgxMaskDirective],
  providers: [provideNgxMask({})],
  templateUrl: './update.component.html',
  styleUrl: './update.component.scss'
})
export default class EmployeeUpdatePageComponent implements OnInit {
  private readonly formData = viewChild<NgForm>('formData');
  private readonly formAuth = viewChild<NgForm>('formAuth');
  private readonly route = inject(ActivatedRoute);
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
    role: ''
  };

  isEditing = this.route.snapshot.params.id > 0;
  superiors: any = [];
  phones: any = [];
  phonePrefix = '';
  phoneNumber = '';
  phoneType = '';

  async ngOnInit() {
    const loggedUserId = this.authService.getUserData().id;
    const superiors = await this.employeeService.getAll();

    //Remove myself from the superior's list
    this.superiors = superiors.filter((sup: any) => sup.id !== loggedUserId);
  }

  //prettier-ignore
  getPhoneType (type: string) {
    return type === "1"
      ? "Home"
      : type === "2"
        ? "Cellular"
        : "Work"
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

  create() {
    console.log(this.formData()?.valid);
    console.log(this.formAuth()?.valid);
  }
}
