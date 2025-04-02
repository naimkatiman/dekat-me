import Link from "next/link";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { categories } from "@/data/mockData";

export default function CategoriesPage() {
  return (
    <div className="container mx-auto py-8 px-4">
      <div className="text-center mb-10">
        <h1 className="text-3xl font-bold mb-2">Business Categories</h1>
        <p className="text-muted-foreground max-w-2xl mx-auto">
          Explore businesses in Malaysia by category. Find everything from restaurants and retail stores to professional services and more.
        </p>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {categories.map((category) => (
          <Card key={category.id} className="flex flex-col h-full">
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <span className="text-2xl">{category.icon}</span>
                <span>{category.name}</span>
              </CardTitle>
              <CardDescription>{category.description}</CardDescription>
            </CardHeader>
            <CardContent className="flex-grow">
              <p className="text-sm text-muted-foreground">
                {category.count} businesses
              </p>
            </CardContent>
            <CardFooter>
              <Button asChild variant="outline" className="w-full">
                <Link href={`/category/${category.slug}`}>
                  View Businesses
                </Link>
              </Button>
            </CardFooter>
          </Card>
        ))}
      </div>

      <div className="mt-12 text-center">
        <p className="text-muted-foreground mb-4">
          Don't see your business category?
        </p>
        <Button asChild>
          <Link href="/submit-business">Add Your Business</Link>
        </Button>
      </div>
    </div>
  );
}
