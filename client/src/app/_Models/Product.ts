import { Photo } from "./Photo";

export interface Product {
  productId: number;
  name: string;
  description: string;
  insertedBy: number;
  insertedOn: Date;
  categoryId: number;
  price: number;
  isSale: boolean;
  owner: string;
  address: string;
  categoryName: string;
  photos: Photo[]
}