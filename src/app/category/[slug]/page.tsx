import Link from "next/link";
import { notFound } from "next/navigation";
import { BiMapPin, BiStar, BiFilter, BiSort } from "react-icons/bi";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { categories, businesses } from "@/data/mockData";

// Generate static params for all category slugs
export function generateStaticParams() {
  return categories.map((category) => ({
    slug: category.slug,
  }));
}

// Helper function to find a category by slug
function getCategoryBySlug(slug: string) {
  return categories.find((category) => category.slug === slug);
}

// Helper function to get businesses in a category
function getBusinessesByCategory(categoryName: string) {
  return businesses.filter((business) => business.category === categoryName);
}

export default function CategoryPage({ params }: { params: { slug: string } }) {
  const category = getCategoryBySlug(params.slug);

  if (!category) {
    notFound();
  }

  const categoryBusinesses = getBusinessesByCategory(category.name);

  return (
    <div className="container mx-auto py-8 px-4">
      {/* Category Header */}
      <div className="text-center mb-10">
        <div className="flex justify-center items-center gap-2 mb-2">
          <span className="text-3xl">{category.icon}</span>
          <h1 className="text-3xl font-bold">{category.name}</h1>
        </div>
        <p className="text-muted-foreground max-w-2xl mx-auto">
          {category.description}
        </p>
      </div>

      {/* Filters (these would be functional in a real app) */}
      <div className="bg-background border rounded-lg p-4 mb-8 flex flex-col sm:flex-row gap-4 justify-between">
        <div className="flex gap-2 flex-wrap">
          <Button variant="outline" size="sm" className="flex items-center gap-1">
            <BiFilter className="h-4 w-4" />
            Filter
          </Button>
          <Button variant="outline" size="sm" className="flex items-center gap-1">
            <BiSort className="h-4 w-4" />
            Sort
          </Button>
          <Button variant="outline" size="sm">Rating 4+</Button>
          <Button variant="outline" size="sm">Open Now</Button>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">
            {categoryBusinesses.length} businesses found
          </p>
        </div>
      </div>

      {/* Business Listings */}
      {categoryBusinesses.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
          {categoryBusinesses.map((business) => (
            <Card key={business.id} className="flex flex-col h-full overflow-hidden">
              <div className="aspect-video relative overflow-hidden">
                <img
                  src={business.images[0].url}
                  alt={business.images[0].alt || business.name}
                  className="object-cover w-full h-full transition-transform hover:scale-105 duration-300"
                />
                {business.isPremium && (
                  <div className="absolute top-2 right-2 bg-yellow-500 text-white text-xs px-2 py-1 rounded-full">
                    Premium
                  </div>
                )}
              </div>
              <CardHeader className="pb-2">
                <CardTitle className="text-lg">{business.name}</CardTitle>
                <CardDescription className="flex justify-between items-center">
                  <Badge variant="outline" className="text-xs font-normal">
                    {business.subcategory || business.category}
                  </Badge>
                  <span className="flex items-center text-yellow-500">
                    <BiStar className="mr-1" />
                    {business.rating} ({business.reviewCount})
                  </span>
                </CardDescription>
              </CardHeader>
              <CardContent className="pb-4 flex-grow">
                <p className="text-sm text-muted-foreground mb-2 line-clamp-2">
                  {business.shortDescription || `${business.description.substring(0, 100)}...`}
                </p>
                <p className="text-muted-foreground text-sm flex items-center">
                  <BiMapPin className="mr-1 shrink-0" />
                  {business.address.formattedAddress || `${business.address.city}, ${business.address.state}`}
                </p>
              </CardContent>
              <CardFooter>
                <Button asChild variant="outline" size="sm" className="w-full">
                  <Link href={`/business/${business.id}`}>View Details</Link>
                </Button>
              </CardFooter>
            </Card>
          ))}
        </div>
      ) : (
        <div className="text-center py-12">
          <h2 className="text-xl font-semibold mb-2">No businesses found</h2>
          <p className="text-muted-foreground mb-6">
            We couldn't find any businesses in this category yet.
          </p>
          <Button asChild>
            <Link href="/submit-business">Add Your Business</Link>
          </Button>
        </div>
      )}

      {/* Pagination (this would be functional in a real app) */}
      {categoryBusinesses.length > 0 && (
        <div className="flex justify-center mt-8">
          <div className="flex gap-2">
            <Button variant="outline" size="sm" disabled>Previous</Button>
            <Button variant="outline" size="sm" className="bg-primary text-primary-foreground hover:bg-primary/90">1</Button>
            <Button variant="outline" size="sm">2</Button>
            <Button variant="outline" size="sm">3</Button>
            <Button variant="outline" size="sm">Next</Button>
          </div>
        </div>
      )}

      {/* CTA Section */}
      <div className="mt-16 text-center">
        <h2 className="text-xl font-bold mb-2">Own a {category.name} Business?</h2>
        <p className="text-muted-foreground mb-6 max-w-2xl mx-auto">
          Join dekat.me to reach more customers and grow your business in Malaysia.
        </p>
        <Button asChild size="lg">
          <Link href="/submit-business">Add Your Business</Link>
        </Button>
      </div>
    </div>
  );
}
