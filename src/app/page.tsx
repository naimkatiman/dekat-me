import Link from "next/link";
import { BiSearch, BiMapPin, BiStore, BiStar } from "react-icons/bi";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { featuredBusinesses, categories } from "@/data/mockData";

export default function Home() {
  return (
    <div className="flex flex-col min-h-screen">
      {/* Hero Section */}
      <section className="relative bg-blue-600 text-white py-16 md:py-24">
        <div className="container px-4 md:px-6 mx-auto">
          <div className="max-w-3xl mx-auto text-center">
            <h1 className="text-3xl md:text-5xl font-bold mb-4">
              Discover Local Businesses in Malaysia
            </h1>
            <p className="text-lg md:text-xl mb-8 text-blue-100">
              Find the best services, restaurants, shops and more near you.
            </p>
            <div className="relative max-w-2xl mx-auto">
              <form className="flex w-full max-w-2xl items-center space-x-2 bg-white rounded-lg p-2 shadow-lg">
                <BiSearch className="h-5 w-5 mx-2 text-gray-500" />
                <Input
                  type="search"
                  placeholder="Search for businesses..."
                  className="flex-1 border-none shadow-none focus-visible:ring-0 focus-visible:ring-offset-0"
                />
                <Button type="submit" className="shrink-0 h-10">
                  Search
                </Button>
              </form>
              <div className="flex justify-center mt-4 gap-4 text-sm text-blue-100">
                <p className="flex items-center">
                  <BiMapPin className="mr-1" /> Near Me
                </p>
                <p className="flex items-center">
                  <BiStore className="mr-1" /> 10,000+ Businesses
                </p>
                <p className="flex items-center">
                  <BiStar className="mr-1" /> Verified Reviews
                </p>
              </div>
            </div>
          </div>
        </div>
        <div className="absolute bottom-0 left-0 right-0 h-16 bg-gradient-to-t from-background to-transparent" />
      </section>

      {/* Featured Businesses Section */}
      <section className="py-12 md:py-16">
        <div className="container px-4 md:px-6 mx-auto">
          <div className="flex justify-between items-center mb-8">
            <div>
              <h2 className="text-2xl md:text-3xl font-bold">Featured Businesses</h2>
              <p className="text-muted-foreground">Highlighted businesses across Malaysia</p>
            </div>
            <Button variant="outline" asChild>
              <Link href="/businesses">View All</Link>
            </Button>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
            {featuredBusinesses.map((business) => (
              <Card key={business.id} className="overflow-hidden h-full">
                <div className="aspect-video relative overflow-hidden">
                  <img
                    src={business.image}
                    alt={business.name}
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
                    <span>{business.category}</span>
                    <span className="flex items-center text-yellow-500">
                      <BiStar className="mr-1" />
                      {business.rating} ({business.reviews})
                    </span>
                  </CardDescription>
                </CardHeader>
                <CardContent className="pb-4">
                  <p className="text-muted-foreground text-sm flex items-center">
                    <BiMapPin className="mr-1 shrink-0" /> {business.address}
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
        </div>
      </section>

      {/* Categories Section */}
      <section className="py-12 md:py-16 bg-muted/50">
        <div className="container px-4 md:px-6 mx-auto">
          <div className="text-center mb-10">
            <h2 className="text-2xl md:text-3xl font-bold mb-2">Browse by Category</h2>
            <p className="text-muted-foreground">Find the perfect business for your needs</p>
          </div>
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
            {categories.map((category) => (
              <Link
                key={category.id}
                href={`/category/${category.slug}`}
                className="bg-background rounded-lg border p-4 flex flex-col items-center text-center hover:border-primary transition-colors"
              >
                <span className="text-3xl mb-2">{category.icon}</span>
                <h3 className="font-medium">{category.name}</h3>
                <p className="text-sm text-muted-foreground">{category.count} businesses</p>
              </Link>
            ))}
          </div>
          <div className="text-center mt-8">
            <Button asChild variant="outline">
              <Link href="/categories">View All Categories</Link>
            </Button>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-12 md:py-16">
        <div className="container px-4 md:px-6 mx-auto">
          <div className="bg-blue-600 text-white rounded-xl p-8 md:p-10">
            <div className="max-w-3xl mx-auto text-center">
              <h2 className="text-2xl md:text-3xl font-bold mb-4">
                Own a Business in Malaysia?
              </h2>
              <p className="text-lg mb-6 text-blue-100">
                Join thousands of businesses on dekat.me and reach more customers.
                Add your business for free or upgrade to premium for enhanced visibility.
              </p>
              <div className="flex flex-col sm:flex-row gap-4 justify-center">
                <Button size="lg" asChild className="bg-white text-blue-600 hover:bg-blue-50">
                  <Link href="/submit-business">Add Your Business</Link>
                </Button>
                <Button size="lg" variant="outline" asChild className="bg-transparent border-white text-white hover:bg-blue-700">
                  <Link href="/subscription">Learn About Premium</Link>
                </Button>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
