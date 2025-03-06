// angular import
import { Component, inject } from '@angular/core';

// project import
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from 'src/app/core/services/employee.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { FilterComponent } from './filter/filter.component';
import { GridComponent } from './grid/grid.component';

@Component({
  selector: 'app-employees-list-page',
  standalone: true,
  imports: [SharedModule, FilterComponent, GridComponent],
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export default class EmployeeListPageComponent {
  private readonly employeeService = inject(EmployeeService);
  private readonly toastr = inject(ToastrService);
  private readonly router = inject(Router);
  employees: any[] = [];

  async ngOnInit() {
    await this.search({});
  }

  async search(filter: any) {
    this.employees = await this.employeeService.filter(filter.firstName, filter.lastName);
  }

  async delete(id: number) {
    const response = await this.employeeService.delete(id);

    if (!response.ok) {
      this.toastr.error('There was an error trying to delete the employee. Please try again later');
      console.error(await response.json());
      return;
    }

    await this.search({});
  }
}
