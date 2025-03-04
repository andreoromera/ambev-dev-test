import { Component, input } from '@angular/core';
import { SharedModule } from "src/app/theme/shared/shared.module";

@Component({
  selector: 'app-grid',
  imports: [SharedModule],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.scss'
})
export class GridComponent {
  employees = input<any[]>();
}
