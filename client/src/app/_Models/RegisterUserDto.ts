export interface RegisterUserDto{
  fullname: string;
  username: string;
  email: string;
  password: string;
  street: string;
  city: string;
  state: string;
  phone: string;
  dateOfBirth: Date;
  role: number;
  isActive: number;
}
