"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useAuth } from "@/contexts/AuthContext";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { businesses } from "@/data/mockData";
import AdminSidebar from "@/components/AdminSidebar";

export default function AdminDashboard() {
  const router = useRouter();
  const { user, loading } = useAuth();
  const [stats, setStats] = useState({
    totalBusinesses: 0,
    premiumBusinesses: 0,
    pendingApproval: 0,
    totalUsers: 0
  });

  useEffect(() => {
    // Redirect if not admin
    if (!loading && (!user || user.user_metadata?.role !== 'admin')) {
      router.push('/login');
    }

    // Calculate stats from mock data
    if (businesses) {
      setStats({
        totalBusinesses: businesses.length,
        premiumBusinesses: businesses.filter(b => b.isPremium).length,
        pendingApproval: 3, // Mock data
        totalUsers: 56 // Mock data
      });
    }
  }, [user, loading, router]);

  if (loading) {
    return <div className="flex items-center justify-center min-h-screen">Loading...</div>;
  }

  if (!user || user.user_metadata?.role !== 'admin') {
    return null; // Will redirect via useEffect
  }

  return (
    <div className="container mx-auto py-8 px-4">
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <AdminSidebar />

        <div className="md:col-span-3 space-y-6">
          <div>
            <h1 className="text-3xl font-bold mb-2">Admin Dashboard</h1>
            <p className="text-muted-foreground">
              Manage your business directory and users
            </p>
          </div>

          {/* Stats Cards */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
            <Card>
              <CardHeader className="pb-2">
                <CardTitle className="text-2xl font-bold">{stats.totalBusinesses}</CardTitle>
                <CardDescription>Total Businesses</CardDescription>
              </CardHeader>
              <CardContent className="pt-0">
                <p className="text-xs text-muted-foreground">
                  All businesses in the directory
                </p>
              </CardContent>
            </Card>

            <Card>
              <CardHeader className="pb-2">
                <CardTitle className="text-2xl font-bold">{stats.premiumBusinesses}</CardTitle>
                <CardDescription>Premium Businesses</CardDescription>
              </CardHeader>
              <CardContent className="pt-0">
                <p className="text-xs text-muted-foreground">
                  Businesses with premium subscription
                </p>
              </CardContent>
            </Card>

            <Card>
              <CardHeader className="pb-2">
                <CardTitle className="text-2xl font-bold">{stats.pendingApproval}</CardTitle>
                <CardDescription>Pending Approval</CardDescription>
              </CardHeader>
              <CardContent className="pt-0">
                <p className="text-xs text-muted-foreground">
                  New business listings to review
                </p>
              </CardContent>
            </Card>

            <Card>
              <CardHeader className="pb-2">
                <CardTitle className="text-2xl font-bold">{stats.totalUsers}</CardTitle>
                <CardDescription>Total Users</CardDescription>
              </CardHeader>
              <CardContent className="pt-0">
                <p className="text-xs text-muted-foreground">
                  Registered users in the system
                </p>
              </CardContent>
            </Card>
          </div>

          {/* Recent Activity */}
          <Card>
            <CardHeader>
              <CardTitle>Recent Activity</CardTitle>
              <CardDescription>
                Latest actions and updates in the system
              </CardDescription>
            </CardHeader>
            <CardContent>
              <Tabs defaultValue="businesses">
                <TabsList className="mb-4">
                  <TabsTrigger value="businesses">Business Updates</TabsTrigger>
                  <TabsTrigger value="users">User Activity</TabsTrigger>
                </TabsList>

                <TabsContent value="businesses">
                  <table className="w-full">
                    <thead>
                      <tr className="border-b">
                        <th className="text-left py-2 font-medium">Business</th>
                        <th className="text-left py-2 font-medium">Action</th>
                        <th className="text-left py-2 font-medium">Date</th>
                        <th className="text-left py-2 font-medium">Status</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">Kopitiams Coffee House</td>
                        <td className="py-2">Updated Profile</td>
                        <td className="py-2">June 23, 2023</td>
                        <td className="py-2"><span className="text-green-600">Approved</span></td>
                      </tr>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">Selangor Handicrafts</td>
                        <td className="py-2">New Registration</td>
                        <td className="py-2">June 22, 2023</td>
                        <td className="py-2"><span className="text-amber-600">Pending</span></td>
                      </tr>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">KL Electronics</td>
                        <td className="py-2">Subscription Upgraded</td>
                        <td className="py-2">June 20, 2023</td>
                        <td className="py-2"><span className="text-green-600">Premium</span></td>
                      </tr>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">JB Auto Repairs</td>
                        <td className="py-2">New Registration</td>
                        <td className="py-2">June 19, 2023</td>
                        <td className="py-2"><span className="text-amber-600">Pending</span></td>
                      </tr>
                    </tbody>
                  </table>
                </TabsContent>

                <TabsContent value="users">
                  <table className="w-full">
                    <thead>
                      <tr className="border-b">
                        <th className="text-left py-2 font-medium">User</th>
                        <th className="text-left py-2 font-medium">Action</th>
                        <th className="text-left py-2 font-medium">Date</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">Ahmad Rahman</td>
                        <td className="py-2">Posted a review</td>
                        <td className="py-2">June 24, 2023</td>
                      </tr>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">Mei Ling</td>
                        <td className="py-2">Registered account</td>
                        <td className="py-2">June 22, 2023</td>
                      </tr>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">David Tan</td>
                        <td className="py-2">Updated profile</td>
                        <td className="py-2">June 21, 2023</td>
                      </tr>
                      <tr className="border-b hover:bg-muted/50">
                        <td className="py-2">Nurul Izzah</td>
                        <td className="py-2">Posted a review</td>
                        <td className="py-2">June 20, 2023</td>
                      </tr>
                    </tbody>
                  </table>
                </TabsContent>
              </Tabs>
            </CardContent>
            <CardFooter>
              <Button variant="outline" className="w-full" asChild>
                <Link href="/admin/activity">View All Activity</Link>
              </Button>
            </CardFooter>
          </Card>

          {/* Quick Actions */}
          <Card>
            <CardHeader>
              <CardTitle>Quick Actions</CardTitle>
              <CardDescription>
                Common administrative tasks
              </CardDescription>
            </CardHeader>
            <CardContent className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <Button asChild>
                <Link href="/admin/businesses/new">Add New Business</Link>
              </Button>
              <Button asChild>
                <Link href="/admin/businesses/pending">Review Pending Businesses</Link>
              </Button>
              <Button asChild>
                <Link href="/admin/users">Manage Users</Link>
              </Button>
              <Button asChild>
                <Link href="/admin/categories">Manage Categories</Link>
              </Button>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
