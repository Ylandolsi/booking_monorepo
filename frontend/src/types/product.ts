interface Product {
  id: string;
  title: string;
  subtitle: string;
  price: string;
  coverImage: string;
  ctaText: string;
  type: 'booking' | 'digital';
  description?: string;
}

export type { Product };
