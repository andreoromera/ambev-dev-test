import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private baseAddress = 'https://localhost:7199';

  async filter(firstName?: string, lastName?: string) {
    try {
      const headers = new Headers();
      headers.append(
        'Authorization',
        `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZ2l2ZW5fbmFtZSI6IkpvaG4iLCJmYW1pbHlfbmFtZSI6IkRvZSIsImVtYWlsIjoiam9obkBhbWJldi5jb20uYnIiLCJyb2xlIjoiQWRtaW4iLCJuYmYiOjE3NDEwMjcwNzUsImV4cCI6MTc0MTAzMDY3NSwiaWF0IjoxNzQxMDI3MDc1fQ.BaeCjIGRQLNLMnovwHO4aLV-mRujZLSHMSgTPy3PWjg`
      );

      const params = new URLSearchParams();
      if (firstName) params.append('firstname', firstName);
      if (lastName) params.append('lastname', lastName);

      const response = await fetch(`${this.baseAddress}/employee/search?${params}`, { headers: headers });

      if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
      }

      return await response.json();
    } catch (err) {
      console.error(err.message);
    }
  }
}
