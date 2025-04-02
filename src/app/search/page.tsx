"use client";

import { useState, useEffect } from "react";
import { useSearchParams } from "next/navigation";
import Link from "next/link";
import { BiMapPin, BiStar, BiFilter, BiSort, BiX } from "react-icons/bi";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Checkbox } from "@/components/ui/checkbox";
import { Slider } from "@/components/ui/slider";
import { businesses, categories } from "@/data/mockData";
import type { Business } from "@/types";

export default function SearchPage() {
  const searchParams = useSearchParams();
  const searchQuery = searchParams.get("q") || "";

  // States
  const [results, setResults] = useState<Business[]>([]);
  const [loading, setLoading] = useState(true);
  const [showFilters, setShowFilters] = useState(false);

  // Filters
  const [selectedCategory, setSelectedCategory] = useState<string>("");
  const [minRating, setMinRating] = useState<number>(0);
  const [isPremiumOnly, setIsPremiumOnly] = useState<boolean>(false);
  const [maxDistance, setMaxDistance] = useState<number>(50); // km
  const [sortBy, setSortBy] = useState<string>("relevance");

  useEffect(() => {
    // Simulate search API call with filters
    setLoading(true);

    // Simple search function that filters through our mock data
    const searchResults = businesses.filter(business => {
      // Search by name or description
      const matchesSearch =
        searchQuery === "" ||
        business.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        (business.description && business.description.toLowerCase().includes(searchQuery.toLowerCase())) ||
        (business.tags && business.tags.some(tag => tag.toLowerCase().includes(searchQuery.toLowerCase())));

      // Category filter
      const matchesCategory = selectedCategory === "" || business.category === selectedCategory;

      // Rating filter
      const matchesRating = (business.rating || 0) >= minRating;

      // Premium filter
      const matchesPremium = !isPremiumOnly || business.isPremium;

      // We would use real geolocation here, but for now just return true for distance
      const matchesDistance = true;

      return matchesSearch && matchesCategory && matchesRating && matchesPremium && matchesDistance;
    });

    // Sort results
    const sortedResults = [...searchResults].sort((a, b) => {
      if (sortBy === "rating") {
        return (b.rating || 0) - (a.rating || 0);
      } else if (sortBy === "newest") {
        return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
      }
      // Default sort by relevance (for demo, we'll just use alphabetical order)
      return a.name.localeCompare(b.name);
    });

    // Simulate network delay
    setTimeout(() => {
      setResults(sortedResults);
      setLoading(false);
    }, 500);
  }, [searchQuery, selectedCategory, minRating, isPremiumOnly, maxDistance, sortBy]);

  return (
    <div className="container py-8 px-4">
      <div className="flex flex-col md:flex-row gap-6">
        {/* Filters Sidebar - Desktop */}
        <div className="hidden md:block w-64 shrink-0">
          <div className="sticky top-20 space-y-6">
            <div>
              <h2 className="font-semibold mb-2">Categories</h2>
              <div className="space-y-2">
                <div className="flex items-center">
                  <Checkbox
                    id="all-categories"
                    checked={selectedCategory === ""}
                    onCheckedChange={() => setSelectedCategory("")}
                  />
                  <label htmlFor="all-categories" className="ml-2 text-sm">All Categories</label>
                </div>
                {categories.map((category) => (
                  <div key={category.id} className="flex items-center">
                    <Checkbox
                      id={`category-${category.id}`}
                      checked={selectedCategory === category.name}
                      onCheckedChange={() => setSelectedCategory(category.name)}
                    />
                    <label htmlFor={`category-${category.id}`} className="ml-2 text-sm flex items-center">
                      <span className="mr-1">{category.icon}</span> {category.name}
                    </label>
                  </div>
                ))}
              </div>
            </div>

            <div>
              <h2 className="font-semibold mb-2">Rating</h2>
              <div className="px-2">
                <Slider
                  defaultValue={[0]}
                  max={5}
                  step={1}
                  value={[minRating]}
                  onValueChange={(value) => setMinRating(value[0])}
                />
                <div className="flex justify-between mt-2 text-sm">
                  <span>Any</span>
                  <span>⭐ {minRating}+</span>
                </div>
              </div>
            </div>

            <div>
              <div className="flex items-center">
                <Checkbox
                  id="premium-filter"
                  checked={isPremiumOnly}
                  onCheckedChange={(checked) => setIsPremiumOnly(checked as boolean)}
                />
                <label htmlFor="premium-filter" className="ml-2 text-sm">Premium Businesses Only</label>
              </div>
            </div>

            <div>
              <h2 className="font-semibold mb-2">Sort By</h2>
              <div className="space-y-2">
                <div className="flex items-center">
                  <Checkbox
                    id="sort-relevance"
                    checked={sortBy === "relevance"}
                    onCheckedChange={() => setSortBy("relevance")}
                  />
                  <label htmlFor="sort-relevance" className="ml-2 text-sm">Relevance</label>
                </div>
                <div className="flex items-center">
                  <Checkbox
                    id="sort-rating"
                    checked={sortBy === "rating"}
                    onCheckedChange={() => setSortBy("rating")}
                  />
                  <label htmlFor="sort-rating" className="ml-2 text-sm">Highest Rated</label>
                </div>
                <div className="flex items-center">
                  <Checkbox
                    id="sort-newest"
                    checked={sortBy === "newest"}
                    onCheckedChange={() => setSortBy("newest")}
                  />
                  <label htmlFor="sort-newest" className="ml-2 text-sm">Newest</label>
                </div>
              </div>
            </div>

            <Button
              variant="outline"
              className="w-full"
              onClick={() => {
                setSelectedCategory("");
                setMinRating(0);
                setIsPremiumOnly(false);
                setMaxDistance(50);
                setSortBy("relevance");
              }}
            >
              Reset Filters
            </Button>
          </div>
        </div>

        {/* Main Content */}
        <div className="flex-1">
          {/* Mobile Filters Toggle */}
          <div className="md:hidden mb-4">
            <Button
              variant="outline"
              className="w-full flex items-center justify-center"
              onClick={() => setShowFilters(!showFilters)}
            >
              <BiFilter className="mr-2" />
              {showFilters ? 'Hide Filters' : 'Show Filters'}
            </Button>

            {showFilters && (
              <div className="mt-4 p-4 border rounded-lg space-y-4">
                <div>
                  <h2 className="font-semibold mb-2">Categories</h2>
                  <div className="grid grid-cols-2 gap-2">
                    {categories.slice(0, 6).map((category) => (
                      <Button
                        key={category.id}
                        variant={selectedCategory === category.name ? "default" : "outline"}
                        size="sm"
                        className="justify-start"
                        onClick={() => setSelectedCategory(selectedCategory === category.name ? "" : category.name)}
                      >
                        <span className="mr-1">{category.icon}</span> {category.name}
                      </Button>
                    ))}
                  </div>
                </div>

                <div>
                  <h2 className="font-semibold mb-2">Rating ({minRating}+)</h2>
                  <Slider
                    defaultValue={[0]}
                    max={5}
                    step={1}
                    value={[minRating]}
                    onValueChange={(value) => setMinRating(value[0])}
                  />
                </div>

                <div className="flex justify-between">
                  <Button
                    variant={isPremiumOnly ? "default" : "outline"}
                    size="sm"
                    onClick={() => setIsPremiumOnly(!isPremiumOnly)}
                  >
                    Premium Only
                  </Button>

                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => {
                      setSelectedCategory("");
                      setMinRating(0);
                      setIsPremiumOnly(false);
                    }}
                  >
                    <BiX className="mr-1" /> Reset
                  </Button>
                </div>
              </div>
            )}
          </div>

          {/* Search Header */}
          <div className="mb-6">
            <h1 className="text-2xl font-bold mb-2">
              {searchQuery ? `Search results for "${searchQuery}"` : "All Businesses"}
            </h1>
            <div className="flex justify-between items-center">
              <p className="text-muted-foreground">
                {results.length} {results.length === 1 ? 'business' : 'businesses'} found
              </p>

              <div className="flex items-center gap-2">
                <span className="text-sm text-muted-foreground">Sort:</span>
                <select
                  className="text-sm border rounded-md p-1"
                  value={sortBy}
                  onChange={(e) => setSortBy(e.target.value)}
                >
                  <option value="relevance">Relevance</option>
                  <option value="rating">Highest Rated</option>
                  <option value="newest">Newest</option>
                </select>
              </div>
            </div>
          </div>

          {/* Search Results */}
          {loading ? (
            <div className="text-center py-12">
              <p className="text-lg text-muted-foreground">Loading results...</p>
            </div>
          ) : results.length > 0 ? (
            <div className="space-y-4">
              {results.map((business) => (
                <Card key={business.id} className="overflow-hidden">
                  <div className="flex flex-col md:flex-row">
                    <div className="md:w-1/3 aspect-video md:aspect-auto">
                      <img
                        src={business.images[0].url}
                        alt={business.name}
                        className="w-full h-full object-cover"
                      />
                    </div>
                    <div className="flex-1 p-4">
                      <div className="flex justify-between items-start">
                        <div>
                          <h2 className="text-lg font-bold mb-1">{business.name}</h2>
                          <div className="flex items-center gap-2 mb-2">
                            <Badge variant="outline" className="text-xs font-normal">
                              {business.category}
                            </Badge>
                            {business.isPremium && (
                              <Badge className="bg-yellow-500 hover:bg-yellow-600">Premium</Badge>
                            )}
                          </div>
                        </div>
                        <div className="flex items-center text-yellow-500">
                          <BiStar className="mr-1" />
                          <span>{business.rating} ({business.reviewCount})</span>
                        </div>
                      </div>

                      <p className="text-sm text-muted-foreground mb-4 line-clamp-2">
                        {business.shortDescription || business.description.substring(0, 120) + '...'}
                      </p>

                      <div className="flex justify-between items-end">
                        <p className="text-sm text-muted-foreground flex items-center">
                          <BiMapPin className="mr-1 shrink-0" />
                          {business.address.formattedAddress || `${business.address.city}, ${business.address.state}`}
                        </p>
                        <Button asChild variant="outline" size="sm">
                          <Link href={`/business/${business.id}`}>View Details</Link>
                        </Button>
                      </div>
                    </div>
                  </div>
                </Card>
              ))}
            </div>
          ) : (
            <div className="text-center py-12 border rounded-lg">
              <h2 className="text-xl font-semibold mb-2">No businesses found</h2>
              <p className="text-muted-foreground mb-6">
                Try adjusting your search criteria or filters
              </p>
              <Button asChild variant="outline">
                <Link href="/">Browse All Businesses</Link>
              </Button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
