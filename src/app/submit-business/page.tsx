"use client";

import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { categories } from "@/data/mockData";

export const dynamic = "force-static";

export default function SubmitBusinessPage() {
  return (
    <div className="container mx-auto py-8 px-4">
      <div className="max-w-3xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold mb-2">Add Your Business</h1>
          <p className="text-muted-foreground">
            Join the dekat.me business directory and connect with new customers in Malaysia
          </p>
        </div>

        <Card>
          <CardHeader>
            <CardTitle>Business Information</CardTitle>
            <CardDescription>
              This form is available in the full application but disabled in this demo.
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-6">
            <div className="space-y-4">
              <h3 className="text-lg font-medium border-b pb-2">Business Categories</h3>
              <div className="grid grid-cols-2 md:grid-cols-4 gap-2">
                {categories.map((category) => (
                  <div key={category.id} className="border rounded p-2 text-center">
                    <div className="text-2xl mb-1">{category.icon}</div>
                    <div className="text-sm font-medium">{category.name}</div>
                  </div>
                ))}
              </div>
            </div>

            <div className="pt-4 text-center">
              <p className="mb-4 text-muted-foreground">
                In the full application, you can submit your business details, including contact information, address, and more.
              </p>
              <Button asChild>
                <Link href="/">Return to Homepage</Link>
              </Button>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
