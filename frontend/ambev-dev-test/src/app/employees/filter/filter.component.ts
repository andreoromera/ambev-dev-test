import { Component } from '@angular/core';
import { SharedModule } from "src/app/theme/shared/shared.module";

@Component({
  selector: 'app-filter',
  imports: [SharedModule],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.scss'
})
export class FilterComponent {

}
