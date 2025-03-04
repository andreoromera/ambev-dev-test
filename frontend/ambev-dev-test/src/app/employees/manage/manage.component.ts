import { Component } from '@angular/core';
import { RouterModule } from "@angular/router";
import { SharedModule } from "src/app/theme/shared/shared.module";

@Component({
  selector: 'app-employees-manage-page',
  standalone: true,
  imports: [SharedModule, RouterModule],
  templateUrl: './manage.component.html',
  styleUrl: './manage.component.scss'
})
export default class EmployeeManagePageComponent {
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
}
