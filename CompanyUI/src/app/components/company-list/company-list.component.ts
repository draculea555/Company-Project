import { Component, OnInit } from '@angular/core';
import { CompanyService } from '../../services/company.service';
import { ICompany } from '../../services/company.model';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent {
  companies: ICompany[] = [];
  newCompany: ICompany = { id: 0, name: '', isin: '', exchange: '', ticker: '', website: '' };
  updateWebsiteData = { id: 0, website: '' };
  companyById: ICompany = { id: 0, name: '', isin: '', exchange: '', ticker: '', website: '' };
  companyByIsin: ICompany = { id: 0, name: '', isin: '', exchange: '', ticker: '', website: '' };
  errorMessage: string = '';
  companyId: string = '';

  constructor(private companyService: CompanyService) { }

  ngOnInit(): void {
    this.loadCompanies();
  }

  loadCompanies(): void {
    this.companyService.getAllCompanies().subscribe(data => {
      this.companies = data;
    });
  }

  createCompany(): void {
    this.companyService.createCompany(this.newCompany).subscribe(() => {
      alert('Company created!');
      this.loadCompanies();
    });
  }

  updateWebsite(): void {
    this.companyService.updateCompanyWebsite(this.updateWebsiteData.id, this.updateWebsiteData.website).subscribe(() => {
      alert('Website updated!');
      this.loadCompanies();
    });
  }

  getCompanyById(id: number) {
    this.companyService.getCompanyById(id).subscribe({
      next: (data) => {
        this.companyById = data;
        this.errorMessage = '';
      },
      error: (err) => {
        this.errorMessage = err.message;
      }
    });
  }

  getCompanyByIsin(): void {
    this.companyService.getCompanyByIsin(this.companyByIsin.isin).subscribe(data => {
      this.companyByIsin = data;
    });
  }
}
