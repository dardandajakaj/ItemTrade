export interface UpdateProductDto{
  name: string;
  description: string;
  categoryId: number;
  price: number;
  isSale: boolean
}