"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { BiSearch } from "react-icons/bi";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";
import { RiMenu3Line } from "react-icons/ri";

export function Navbar() {
  const router = useRouter();
  const { user, signOut } = useAuth();
  const [searchTerm, setSearchTerm] = useState("");

  const isAdmin = user?.user_metadata?.role === 'admin';
  const userName = user?.user_metadata?.name || user?.email?.split('@')[0] || 'User';

  const handleSignOut = async () => {
    await signOut();
    router.push('/');
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    if (searchTerm.trim()) {
      router.push(`/search?q=${encodeURIComponent(searchTerm)}`);
    }
  };

  return (
    <header className="sticky top-0 z-50 w-full border-b bg-background">
      <div className="container flex h-16 items-center justify-between px-4 md:px-6">
        <div className="flex items-center gap-4">
          <Sheet>
            <SheetTrigger asChild className="md:hidden">
              <Button variant="ghost" size="icon">
                <RiMenu3Line className="h-5 w-5" />
                <span className="sr-only">Toggle menu</span>
              </Button>
            </SheetTrigger>
            <SheetContent side="left" className="w-[300px] sm:w-[350px]">
              <nav className="flex flex-col gap-4 mt-8">
                <Link href="/" className="text-lg font-bold hover:underline">Home</Link>
                <Link href="/categories" className="text-lg hover:underline">Categories</Link>
                <Link href="/map" className="text-lg hover:underline">Map View</Link>
                <Link href="/submit-business" className="text-lg hover:underline">Add Business</Link>
                {isAdmin && (
                  <>
                    <div className="h-px bg-border my-2"></div>
                    <Link href="/admin" className="text-lg text-blue-600 hover:underline">Admin Dashboard</Link>
                    <Link href="/admin/businesses" className="text-lg text-blue-600 hover:underline">Manage Businesses</Link>
                    <Link href="/admin/users" className="text-lg text-blue-600 hover:underline">Manage Users</Link>
                  </>
                )}
              </nav>
            </SheetContent>
          </Sheet>
          <Link href="/" className="flex items-center gap-2">
            <span className="text-xl font-bold tracking-tighter md:text-2xl">dekat.me</span>
          </Link>
          <nav className="hidden md:flex gap-6 ml-6">
            <Link href="/categories" className="text-sm font-medium hover:underline">
              Categories
            </Link>
            <Link href="/map" className="text-sm font-medium hover:underline">
              Map View
            </Link>
            <Link href="/submit-business" className="text-sm font-medium hover:underline">
              Add Business
            </Link>
            {isAdmin && (
              <Link href="/admin" className="text-sm font-medium text-blue-600 hover:underline">
                Admin
              </Link>
            )}
          </nav>
        </div>
        <div className="flex items-center gap-4">
          <form
            className="hidden md:flex relative"
            onSubmit={handleSearch}
          >
            <Input
              type="search"
              placeholder="Search businesses..."
              className="w-[200px] lg:w-[300px]"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
            <Button type="submit" variant="ghost" size="icon" className="absolute right-0">
              <BiSearch className="h-5 w-5" />
              <span className="sr-only">Search</span>
            </Button>
          </form>
          {user ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="icon" className="rounded-full">
                  <Avatar className="h-8 w-8">
                    <AvatarImage src="" alt={userName} />
                    <AvatarFallback>{userName.charAt(0).toUpperCase()}</AvatarFallback>
                  </Avatar>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>My Account</DropdownMenuLabel>
                <DropdownMenuLabel className="text-xs font-normal text-muted-foreground">
                  {user.email}
                </DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem asChild>
                  <Link href="/profile">Profile</Link>
                </DropdownMenuItem>
                <DropdownMenuItem asChild>
                  <Link href="/my-businesses">My Businesses</Link>
                </DropdownMenuItem>
                {isAdmin && (
                  <>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem asChild>
                      <Link href="/admin">Admin Dashboard</Link>
                    </DropdownMenuItem>
                    <DropdownMenuItem asChild>
                      <Link href="/admin/businesses">Manage Businesses</Link>
                    </DropdownMenuItem>
                  </>
                )}
                <DropdownMenuSeparator />
                <DropdownMenuItem
                  onClick={handleSignOut}
                  className="text-red-600 cursor-pointer"
                >
                  Logout
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <div className="flex items-center gap-2">
              <Button variant="outline" size="sm" asChild>
                <Link href="/login">Login</Link>
              </Button>
              <Button size="sm" asChild>
                <Link href="/register">Register</Link>
              </Button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}
