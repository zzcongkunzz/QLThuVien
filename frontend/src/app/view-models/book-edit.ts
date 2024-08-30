export class BookEdit {
  title: string = 'Book Title';
  authorName: string = 'Unkown Author';
  publisherName: string = 'Unkown Publisher';
  description?: string | null = 'No Description';
  categoryName: string = 'Uncategorized';
  publishDate: Date = new Date();
  count: number = 0;
}
