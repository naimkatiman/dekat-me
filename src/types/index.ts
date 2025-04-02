// User related interfaces
export interface User {
  id: string;
  name: string;
  email: string;
  role: 'user' | 'business' | 'admin';
  avatar?: string;
  createdAt: Date;
}

// Business related interfaces
export interface Business {
  id: string;
  userId?: string;
  name: string;
  description: string;
  shortDescription?: string;
  category: string;
  subcategory?: string;
  tags?: string[];
  website?: string;
  phone?: string;
  email?: string;
  address: Address;
  hours?: BusinessHours;
  images: Image[];
  rating?: number;
  reviewCount?: number;
  isPremium: boolean;
  isClaimed: boolean;
  isVerified: boolean;
  features?: string[];
  createdAt: Date;
  updatedAt: Date;
}

export interface Address {
  street: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  latitude?: number;
  longitude?: number;
  formattedAddress?: string;
}

export interface BusinessHours {
  monday?: DayHours;
  tuesday?: DayHours;
  wednesday?: DayHours;
  thursday?: DayHours;
  friday?: DayHours;
  saturday?: DayHours;
  sunday?: DayHours;
}

export interface DayHours {
  isOpen: boolean;
  open: string;
  close: string;
}

export interface Image {
  id: string;
  url: string;
  alt?: string;
  isPrimary?: boolean;
}

// Review related interfaces
export interface Review {
  id: string;
  businessId: string;
  userId: string;
  userName: string;
  userAvatar?: string;
  rating: number;
  comment: string;
  images?: Image[];
  likes: number;
  createdAt: Date;
  updatedAt: Date;
}

// Category related interfaces
export interface Category {
  id: string;
  name: string;
  slug: string;
  description?: string;
  icon?: string;
  image?: string;
  parentId?: string;
  count?: number;
}

// Subscription related interfaces
export interface Subscription {
  id: string;
  businessId: string;
  plan: 'basic' | 'premium';
  startDate: Date;
  endDate: Date;
  isActive: boolean;
  paymentStatus: 'pending' | 'completed' | 'failed';
}

// Spotlight token related interfaces
export interface SpotlightToken {
  id: string;
  businessId: string;
  isRedeemed: boolean;
  redeemedAt?: Date;
  expiresAt?: Date;
  createdAt: Date;
}

// Search related interfaces
export interface SearchFilters {
  category?: string;
  subcategory?: string;
  tag?: string;
  rating?: number;
  distance?: number;
  isPremium?: boolean;
  isOpen?: boolean;
  price?: string;
  sortBy?: 'relevance' | 'rating' | 'distance' | 'newest';
}

export interface SearchResult {
  businesses: Business[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

// For featured businesses on homepage
export interface FeaturedBusiness {
  id: string;
  name: string;
  category: string;
  rating: number;
  reviews: number;
  address: string;
  image: string;
  isPremium: boolean;
}
