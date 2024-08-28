export class Borrow {
  id: string = "no id";
  userId: string = "no userId";
  bookId: string = "no bookId";
  userFullName: string = "no userFullName";
  bookTitle: string = "no bookTitle";
  startTime: Date = new Date();
  expectedReturnTime: Date = new Date();
  actualReturnTime?: Date | null = new Date();
  count: number = 0;
  issuedPenalties: number = 0;
  paidPenalties: number = 0;
}
