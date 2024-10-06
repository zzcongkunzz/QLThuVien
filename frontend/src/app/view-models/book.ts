export class Book {
  id: string = 'no-id';
  title: string = 'Book Title';
  authorName: string = 'Unkown Author';
  publisherName: string = 'Unkown Publisher';
  description?: string | null = 'No Description';
  categoryName: string = 'Uncategorized';
  imageUrl: string = '/images/book.png';
  publishDate: Date = new Date();
  averageRating: number = 3;
  count: number = 0;
}
