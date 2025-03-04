import { Component, output } from '@angular/core';
import { RouterLink } from "@angular/router";
import { SharedModule } from "src/app/theme/shared/shared.module";

@Component({
  selector: 'app-filter',
  imports: [SharedModule, RouterLink],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.scss'
})
export class FilterComponent {
  search = output<any>();
  firstName?: string;
  lastName?: string;

  onSearchClicked() {
    this.search.emit({
      firstName: this.firstName,
      lastName: this.lastName
    });
  }
}
