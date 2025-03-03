// angular import
import { animate, style, transition, trigger } from '@angular/animations';
import { Component, computed, inject } from '@angular/core';

// bootstrap import
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';

// project import
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ChatMsgComponent } from './chat-msg/chat-msg.component';
import { ChatUserListComponent } from './chat-user-list/chat-user-list.component';

@Component({
  selector: 'app-nav-right',
  imports: [SharedModule, ChatUserListComponent, ChatMsgComponent],
  templateUrl: './nav-right.component.html',
  styleUrls: ['./nav-right.component.scss'],
  providers: [NgbDropdownConfig],
  animations: [
    trigger('slideInOutLeft', [
      transition(':enter', [style({ transform: 'translateX(100%)' }), animate('300ms ease-in', style({ transform: 'translateX(0%)' }))]),
      transition(':leave', [animate('300ms ease-in', style({ transform: 'translateX(100%)' }))])
    ]),
    trigger('slideInOutRight', [
      transition(':enter', [style({ transform: 'translateX(-100%)' }), animate('300ms ease-in', style({ transform: 'translateX(0%)' }))]),
      transition(':leave', [animate('300ms ease-in', style({ transform: 'translateX(-100%)' }))])
    ])
  ]
})
export class NavRightComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  // public props
  visibleUserList: boolean;
  chatMessage: boolean;
  friendId!: number;

  username = computed(() => this.authService.getUserData().fullName);

  // constructor
  constructor() {
    this.visibleUserList = false;
    this.chatMessage = false;
  }

  // public method
  // eslint-disable-next-line
  onChatToggle(friendID: any) {
    this.friendId = friendID;
    this.chatMessage = !this.chatMessage;
  }

  logout() {
    this.authService.logout();
    location.href = "/";
  }
}
