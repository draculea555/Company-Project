export interface ICompany {
  id: number;
  name: string;
  exchange: string;
  ticker: string;
  isin: string;
  website?: string; // Optional field (matches C# nullable string)
}
