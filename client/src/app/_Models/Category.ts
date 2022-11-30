import { User } from "./User";

export interface Category {
  categoryId: number;
  name: string;
  vat: number;
  addedBy: number;
  addedOn: Date;
  user?: User;
  products?: any;
}
