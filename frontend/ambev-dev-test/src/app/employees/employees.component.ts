// angular import
import { Component, inject } from '@angular/core';

// project import
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { EmployeeService } from "../core/services/employee.service";
import { FilterComponent } from "./filter/filter.component";
import { ListComponent } from "./list/list.component";

@Component({
  selector: 'app-employees-page',
  standalone: true,
  imports: [SharedModule, FilterComponent, ListComponent],
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export default class EmployeesPageComponent {
  private employeeService = inject(EmployeeService);
  employees: any[] = [];

  async ngOnInit() {
    await this.search({});
  }

  async search(filter: any) {
    this.employees = await this.employeeService.filter(filter.firstName, filter.lastName);
  }
}
