// angular import
import { Component } from '@angular/core';

// project import
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { FilterComponent } from "./filter/filter.component";
import { ListComponent } from "./list/list.component";

@Component({
  selector: 'app-employees-page',
  standalone: true,
  imports: [SharedModule, FilterComponent, ListComponent],
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export default class EmployeesPageComponent {}
