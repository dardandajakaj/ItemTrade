import { Photo } from "./Photo";

export interface ProductDto{
  name: string;
  description: string;
  insertedBy: number;
  insertedOn: string;
  categoryId: number;
  price: number;
  isSale: boolean;
  photos: Photo[];
}