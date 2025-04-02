import { Business, Category, Review } from '@/types';
import { supabase } from './supabase';

// API Functions for businesses
export const getBusinesses = async (): Promise<Business[]> => {
  try {
    const { data, error } = await supabase
      .from('businesses')
      .select('*');
    
    if (error) throw error;
    return data || [];
  } catch (error) {
    console.error('Error fetching businesses:', error);
    return [];
  }
};

export const getBusinessById = async (id: string): Promise<Business | null> => {
  try {
    const { data, error } = await supabase
      .from('businesses')
      .select('*')
      .eq('id', id)
      .single();
    
    if (error) throw error;
    return data;
  } catch (error) {
    console.error(`Error fetching business with ID ${id}:`, error);
    return null;
  }
};

export const getBusinessesByCategory = async (categoryId: string): Promise<Business[]> => {
  try {
    const { data, error } = await supabase
      .from('businesses')
      .select('*')
      .eq('categoryId', categoryId);
    
    if (error) throw error;
    return data || [];
  } catch (error) {
    console.error(`Error fetching businesses by category ${categoryId}:`, error);
    return [];
  }
};

// API Functions for categories
export const getCategories = async (): Promise<Category[]> => {
  try {
    const { data, error } = await supabase
      .from('categories')
      .select('*');
    
    if (error) throw error;
    return data || [];
  } catch (error) {
    console.error('Error fetching categories:', error);
    return [];
  }
};

// API Functions for reviews
export const getReviewsByBusinessId = async (businessId: string): Promise<Review[]> => {
  try {
    const { data, error } = await supabase
      .from('reviews')
      .select('*')
      .eq('businessId', businessId);
    
    if (error) throw error;
    return data || [];
  } catch (error) {
    console.error(`Error fetching reviews for business ${businessId}:`, error);
    return [];
  }
};

export const addReview = async (review: Omit<Review, 'id' | 'createdAt'>): Promise<Review | null> => {
  try {
    const { data, error } = await supabase
      .from('reviews')
      .insert([{ ...review, createdAt: new Date().toISOString() }])
      .select()
      .single();
    
    if (error) throw error;
    return data;
  } catch (error) {
    console.error('Error adding review:', error);
    return null;
  }
};

// Search function
export const searchBusinesses = async (query: string): Promise<Business[]> => {
  try {
    const { data, error } = await supabase
      .from('businesses')
      .select('*')
      .or(`name.ilike.%${query}%, description.ilike.%${query}%`);
    
    if (error) throw error;
    return data || [];
  } catch (error) {
    console.error(`Error searching businesses with query "${query}":`, error);
    return [];
  }
};
