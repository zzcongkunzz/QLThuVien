export class Borrow {
  userId: string = "no userId";
  bookId: string = "no bookId";
  startTime: Date = new Date();
  expectedReturnTime: Date = new Date();
  count: number = 0;
  issuedPenalties: number = 0;
  paidPenalties: number = 0;
}
