import { Component, inject, OnInit, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { PasswordStrengthMeterComponent } from 'angular-password-strength-meter';
import { NgxMaskDirective } from 'ngx-mask';
import { ToastrService } from 'ngx-toastr';
import { MustMatchDirective } from 'src/app/core/directives/mustmatch.directive';
import Employee from 'src/app/core/models/employee';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeService } from 'src/app/core/services/employee.service';
import { RoleService } from 'src/app/core/services/role.service';
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
  private readonly router = inject(Router);
  private readonly formData = viewChild.required<NgForm>('formData');
  private readonly employeeService = inject(EmployeeService);
  private readonly authService = inject(AuthService);
  private readonly roleService = inject(RoleService);
  private readonly toastr = inject(ToastrService);

  employee = new Employee();
  roles: any[] = [];
  superiors: any = [];
  phonePrefix = '';
  phoneNumber = '';
  phoneType = '';
  passwordStrengthPattern = /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/;
  loggedUserRoleId = 0;

  async ngOnInit() {
    const user = this.authService.getUserData();
    this.roles = this.roleService.getRoles();
    this.loggedUserRoleId = this.roleService.getRoleValue(user.role);

    //Remove myself from the superior's list
    const superiors = await this.employeeService.getAll();
    this.superiors = superiors.filter((sup: any) => sup.id !== user.id);
  }

  addPhone() {
    if (this.phonePrefix.trim() !== '' && this.phoneNumber.trim() !== '' && this.phoneType.trim() !== '') {
      this.employee.phones.push({
        phonePrefix: this.phonePrefix,
        phoneNumber: this.phoneNumber,
        phoneType: this.phoneType
      });
      this.clearPhoneForm();
    }
  }

  clearPhoneForm() {
    this.phonePrefix = '';
    this.phoneNumber = '';
    this.phoneType = '';
  }

  async create () {
    const valid =
      this.formData().valid &&
      this.employee.phones.length > 0 &&
      this.employee.roleIsValid(this.loggedUserRoleId);

    if (valid) {
      const response = await this.employeeService.create({
        ...this.employee,
        role: this.employee.role?.name
      });

      if (!response.ok) {
        const json = await response.json();
        if (response.status === 400) {
          if (json.errors) {
            this.toastr.error(Object.values<string>(json.errors)[0]);
          } else if (json.detail) {
            this.toastr.error(json.detail);
          }
          return;
        }

        this.toastr.error('There was an error trying to create the user. Please try again later');
        return;
      }

      this.router.navigate(['/employees']);
    }
  }
}
