import { Component } from '@angular/core';
import { SharedModule } from "src/app/theme/shared/shared.module";

@Component({
  selector: 'app-list',
  imports: [SharedModule],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent {

}
