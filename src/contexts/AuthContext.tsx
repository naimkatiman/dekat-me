'use client';

import type React from 'react';
import { createContext, useContext, useEffect, useState } from 'react';
import type { Session } from '@supabase/supabase-js';
import { supabase, type SupabaseUser } from '@/lib/supabase';

type AuthContextType = {
  session: Session | null;
  user: SupabaseUser | null;
  loading: boolean;
  signUp: (email: string, password: string, name: string) => Promise<{ error: any }>;
  signIn: (email: string, password: string) => Promise<{ error: any }>;
  signOut: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [session, setSession] = useState<Session | null>(null);
  const [user, setUser] = useState<SupabaseUser | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // For static export purposes, we're using a simulated initialization
    const simulateInitialUser = async () => {
      setLoading(true);
      // Check local storage for user data (simulating Supabase behavior)
      const storedSession = localStorage.getItem('dekat_me_session');

      if (storedSession) {
        try {
          const parsedSession = JSON.parse(storedSession);
          setSession(parsedSession.session);
          setUser(parsedSession.user);
        } catch (error) {
          console.error('Failed to parse stored session:', error);
          localStorage.removeItem('dekat_me_session');
        }
      }

      setLoading(false);
    };

    // In a real app with dynamic server, we'd use Supabase's auth state change
    /*
    const { data: { subscription } } = supabase.auth.onAuthStateChange(
      (_event, newSession) => {
        setSession(newSession);
        setUser(newSession?.user ?? null);
        setLoading(false);
      }
    );
    return () => {
      subscription.unsubscribe();
    };
    */

    simulateInitialUser();
  }, []);

  const signUp = async (email: string, password: string, name: string) => {
    // In a real app, this would call Supabase signUp
    /*
    const { data, error } = await supabase.auth.signUp({
      email,
      password,
      options: {
        data: {
          name,
        },
      },
    });
    */

    // Simulate signUp for static export
    try {
      // Simulate a network request
      await new Promise(resolve => setTimeout(resolve, 1000));

      if (email === 'admin@dekat.me' || email === 'user@dekat.me') {
        // For demo, use these test emails
        const mockUser: SupabaseUser = {
          id: '12345',
          email,
          user_metadata: {
            name,
          }
        };

        const mockSession = {
          access_token: 'mock_token',
          refresh_token: 'mock_refresh',
          expires_at: Date.now() + 3600,
          user: mockUser
        };

        localStorage.setItem('dekat_me_session', JSON.stringify({
          session: mockSession,
          user: mockUser
        }));

        setSession(mockSession as unknown as Session);
        setUser(mockUser);

        return { error: null };
      } else {
        // Simulate registration errors
        return { error: { message: 'Registration failed. Use admin@dekat.me or user@dekat.me for demo.' } };
      }
    } catch (error) {
      return { error };
    }
  };

  const signIn = async (email: string, password: string) => {
    // In a real app, this would call Supabase signIn
    /*
    const { data, error } = await supabase.auth.signInWithPassword({
      email,
      password,
    });
    */

    // Simulate signIn for static export
    try {
      // Simulate a network request
      await new Promise(resolve => setTimeout(resolve, 1000));

      if ((email === 'admin@dekat.me' && password === 'password123') ||
          (email === 'user@dekat.me' && password === 'password123')) {

        const isAdmin = email === 'admin@dekat.me';

        const mockUser: SupabaseUser = {
          id: isAdmin ? 'admin-123' : 'user-456',
          email,
          user_metadata: {
            name: isAdmin ? 'Admin User' : 'Regular User',
            role: isAdmin ? 'admin' : 'user'
          }
        };

        const mockSession = {
          access_token: 'mock_token',
          refresh_token: 'mock_refresh',
          expires_at: Date.now() + 3600,
          user: mockUser
        };

        localStorage.setItem('dekat_me_session', JSON.stringify({
          session: mockSession,
          user: mockUser
        }));

        setSession(mockSession as unknown as Session);
        setUser(mockUser);

        return { error: null };
      } else {
        // Simulate login errors
        return { error: { message: 'Invalid login credentials. Use admin@dekat.me/password123 or user@dekat.me/password123 for demo.' } };
      }
    } catch (error) {
      return { error };
    }
  };

  const signOut = async () => {
    // In a real app, this would call Supabase signOut
    // await supabase.auth.signOut();

    // Simulate signOut for static export
    localStorage.removeItem('dekat_me_session');
    setSession(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ session, user, loading, signUp, signIn, signOut }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
