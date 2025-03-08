import { Component, inject, OnInit, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
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
  selector: 'app-employees-manage-page',
  standalone: true,
  imports: [SharedModule, RouterModule, NgxMaskDirective, MustMatchDirective, PasswordStrengthMeterComponent],
  providers: [],
  templateUrl: './manage.component.html',
  styleUrl: './manage.component.scss'
})
export default class EmployeeManagePageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly formData = viewChild.required<NgForm>('formData');
  private readonly employeeService = inject(EmployeeService);
  private readonly authService = inject(AuthService);
  private readonly roleService = inject(RoleService);
  private readonly toastr = inject(ToastrService);

  employee?: Employee;
  roles: any[] = [];
  superiors: any = [];
  phonePrefix = '';
  phoneNumber = '';
  phoneType = '';
  passwordStrengthPattern = /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/;
  loggedUserRoleId = 0;
  isEditing = false;

  async ngOnInit() {
    const user = this.authService.getUserData();
    this.roles = this.roleService.getRoles();
    this.loggedUserRoleId = this.roleService.getRoleValue(user.role);

    //Remove myself from the superior's list
    const superiors = await this.employeeService.getAll();
    this.superiors = superiors.filter((sup: any) => sup.id !== user.id);

    if (this.route.snapshot.params.id) {
      this.isEditing = true;
      this.employee = await this.employeeService.getById(this.route.snapshot.params.id);
    } else {
      this.employee = new Employee();
    }
  }

  addPhone() {
    if (this.phonePrefix.trim() !== '' && this.phoneNumber.trim() !== '' && this.phoneType.trim() !== '') {
      this.employee!.phones.push({
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

  //prettier-ignore
  async save() {
    const valid =
      this.formData().valid &&
      this.employee!.phones.length > 0 &&
      this.roleIsValid();

    if (valid) {
      let response;

      if (this.isEditing) {
        response = await this.employeeService.update({
          ...this.employee,
          role: this.employee!.role?.id
        });
      } else {
        response = await this.employeeService.create({
          ...this.employee,
          role: this.employee!.role?.id
        });
      }

      if (!response.ok) {
        const json = await response.json();
        if (response.status === 400) {
          if (json.errors) {
            this.toastr.warning(Object.values<string>(json.errors)[0]);
          } else if (json.detail) {
            this.toastr.warning(json.detail);
          } else {
            console.log(json);
          }
          return;
        }

        this.toastr.warning('There was an error trying to save the employee. Please try again later');
        return;
      }

      this.router.navigate(['/employees']);
    }
  }

  //prettier-ignore
  roleIsValid() {
    return this.employee!.role
        && this.employee!.role.id > 0
        && this.employee!.role.id <= this.loggedUserRoleId;
  }

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.id === c2.id : c1 === c2;
  }
}
