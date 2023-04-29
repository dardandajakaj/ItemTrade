export interface Message{
  id: number,
  conversationId: number,
  senderId: number,
  content: string,
  sentOn: Date
}