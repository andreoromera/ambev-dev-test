// angular import
import { Component, inject } from '@angular/core';

// project import
import { EmployeeService } from "src/app/core/services/employee.service";
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { FilterComponent } from "./filter/filter.component";
import { GridComponent } from "./grid/grid.component";

@Component({
  selector: 'app-employees-list-page',
  standalone: true,
  imports: [SharedModule, FilterComponent, GridComponent],
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export default class EmployeeListPageComponent {
  private employeeService = inject(EmployeeService);
  employees: any[] = [];

  async ngOnInit() {
    await this.search({});
  }

  async search(filter: any) {
    this.employees = await this.employeeService.filter(filter.firstName, filter.lastName);
  }
}
