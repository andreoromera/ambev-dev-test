import { Injectable } from '@angular/core';
import Role from "../models/role";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  constructor() {}

  getRoles (): Role[] {
    return [
      { id: 600, name: 'Admin',  },
      { id: 500, name: 'President' },
      { id: 400, name: 'Director' },
      { id: 300, name: 'Manager' },
      { id: 200, name: 'Coordinator' },
      { id: 100, name: 'Developer' }
    ];
  }

  //prettier-ignore
  getRoleValue(role: string) {
    switch (role) {
      case "Admin": return 600;
      case "President": return 500;
      case "Director": return 400;
      case "Manager": return 300;
      case "Coordinator": return 200;
      case "Developer": return 100;
      default: return 0;
    }
  }

  //prettier-ignore
  getRoleName(role: Number) {
    switch (role) {
      case 600: return 'Admin';
      case 500: return 'President';
      case 400: return 'Director';
      case 300: return 'Manager';
      case 200: return 'Coordinator';
      case 100: return 'Developer';
      default: return 0;
    }
  }
}
