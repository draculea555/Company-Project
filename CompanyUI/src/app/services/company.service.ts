import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { ICompany } from './company.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = 'https://localhost:7073/api/Company'; // Adjust API URL if needed

  constructor(private http: HttpClient) { }

  getAllCompanies(): Observable<ICompany[]> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa('admin:password') // Encode credentials
    });
    return this.http.get<ICompany[]>(`${this.apiUrl}/AllCompanies`, { headers });
  }

  getCompanyById(id: number): Observable<ICompany> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa('admin:password') // Encode credentials
    });
    return this.http.get<ICompany>(`${this.apiUrl}/Id/${id}`, { headers }).pipe(
      catchError(this.handleError));
  }

  getCompanyByIsin(isin: string): Observable<ICompany> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa('admin:password') // Encode credentials
    });
    return this.http.get<ICompany>(`${this.apiUrl}/Isin/${isin}`, { headers });
  }

  createCompany(company: ICompany): Observable<number> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa('admin:password') // Encode credentials
    });
    return this.http.post<number>(`${this.apiUrl}/Create`, company, { headers });
  }

  updateCompany(id: number, company: ICompany): Observable<boolean> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa('admin:password') // Encode credentials
    });
    return this.http.put<boolean>(`${this.apiUrl}/Update/${id}`, company, { headers });
  }

  updateCompanyWebsite(id: number, website: string): Observable<boolean> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Basic ' + btoa('admin:password') // Encode credentials
    });
    return this.http.patch<boolean>(`${this.apiUrl}/UpdateWebsite/${id}`, JSON.stringify(website), { headers });
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 404) {
      return throwError(() => new Error('Company not found.'));
    }
    return throwError(() => new Error('Something went wrong.'));
  }
}
