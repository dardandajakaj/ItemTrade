export interface Photo{
  id?: string,
  belongsTo?: number,
  filename: string,
  isMain: boolean,
  files?: File
}