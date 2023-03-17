export interface Message{
  id: number,
  senderId: number,
  receiverId: number,
  content: string,
  sentOn: Date,
  readOn: Date
}