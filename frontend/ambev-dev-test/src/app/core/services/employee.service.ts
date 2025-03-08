import { inject, Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private authService = inject(AuthService);

  async getById (employeeId: number) {
    try {
      const response = await fetch(`${environment.apiUrl}/employee/${employeeId}`, { headers: this.getHeaders() });

      if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
      }

      return await response.json();
    } catch (err) {
      console.error(err.message);
    }
  }

  async filter(firstName?: string, lastName?: string) {
    try {
      const params = new URLSearchParams();
      if (firstName) params.append('firstname', firstName);
      if (lastName) params.append('lastname', lastName);

      const response = await fetch(`${environment.apiUrl}/employee/search?${params}`, { headers: this.getHeaders() });

      if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
      }

      return await response.json();
    } catch (err) {
      console.error(err.message);
    }
  }

  async getAll() {
    try {
      const response = await fetch(`${environment.apiUrl}/employee/all`, { headers: this.getHeaders() });

      if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
      }

      return await response.json();
    } catch (err) {
      console.error(err.message);
    }
  }

  async create(employee: any) {
    return await fetch(`${environment.apiUrl}/employee`, {
      method: 'POST',
      headers: this.getHeaders(),
      body: JSON.stringify(employee)
    });
  }

  async update(employee: any) {
    return await fetch(`${environment.apiUrl}/employee`, {
      method: 'PUT',
      headers: this.getHeaders(),
      body: JSON.stringify(employee)
    });
  }

  async delete(id: number) {
    return await fetch(`${environment.apiUrl}/employee/${id}`, {
      method: 'DELETE',
      headers: this.getHeaders()
    });
  }

  private getHeaders() {
    const user = this.authService.getUserData()!;

    const headers = new Headers();
    headers.append('Authorization', `Bearer ${user.token}`);
    headers.append('Content-Type', `application/json`);
    return headers;
  }
}
