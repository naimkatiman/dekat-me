import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import ClientBody from "./ClientBody";
import { Navbar } from "@/components/Navbar";
import { Footer } from "@/components/Footer";
import { AuthProvider } from "@/contexts/AuthContext";
import { Toaster } from "@/components/ui/toaster";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "Dekat.me | Find Local Malaysian Businesses Near You",
  description: "Discover and connect with local Malaysian businesses, read reviews, and find services in your area with our modern business directory platform.",
  keywords: "Malaysia, business directory, local businesses, reviews, services, nearby",
  authors: [{ name: "Dekat.me Team" }],
  openGraph: {
    title: "Dekat.me | Find Local Malaysian Businesses Near You",
    description: "Discover and connect with local Malaysian businesses, read reviews, and find services in your area.",
    url: "https://dekat.me",
    siteName: "Dekat.me",
    locale: "en_MY",
    type: "website",
  },
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" className={`${geistSans.variable} ${geistMono.variable}`}>
      <ClientBody>
        <AuthProvider>
          <div className="flex flex-col min-h-screen">
            <Navbar />
            <main className="flex-grow">{children}</main>
            <Footer />
          </div>
          <Toaster />
        </AuthProvider>
      </ClientBody>
    </html>
  );
}
