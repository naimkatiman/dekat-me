"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import dynamic from "next/dynamic";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { Icon } from "leaflet";
import { BiCategory, BiStar, BiFilter } from "react-icons/bi";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import { businesses, categories } from "@/data/mockData";
import { Business } from "@/types";

// Import leaflet CSS - we need to do this in the component due to SSR
const LeafletMapWithNoSSR = dynamic(() => import("@/components/LeafletMap"), {
  ssr: false,
});

// Define the center of Malaysia (approximate)
const MALAYSIA_CENTER = [3.139, 101.6869]; // Kuala Lumpur coordinates
const DEFAULT_ZOOM = 10;

export default function MapPage() {
  const [filteredBusinesses, setFilteredBusinesses] = useState<Business[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<string>("");
  const [isPremiumOnly, setIsPremiumOnly] = useState<boolean>(false);
  const [showFilters, setShowFilters] = useState(false);
  const [selectedBusiness, setSelectedBusiness] = useState<Business | null>(null);

  // Apply filters
  useEffect(() => {
    const filtered = businesses.filter(business => {
      const matchesCategory = selectedCategory === "" || business.category === selectedCategory;
      const matchesPremium = !isPremiumOnly || business.isPremium;
      return matchesCategory && matchesPremium;
    });

    setFilteredBusinesses(filtered);
  }, [selectedCategory, isPremiumOnly]);

  return (
    <div className="container py-4 px-4">
      <div className="mb-6">
        <h1 className="text-2xl font-bold mb-2">Business Map</h1>
        <p className="text-muted-foreground">
          Explore businesses across Malaysia with our interactive map
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {/* Filters Panel */}
        <div className="md:col-span-1">
          <Card>
            <CardHeader className="pb-3">
              <CardTitle className="text-lg flex items-center">
                <BiFilter className="mr-2" /> Filters
              </CardTitle>
              <CardDescription>
                Filter businesses on the map
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div>
                <h3 className="text-sm font-medium mb-2">Categories</h3>
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

              <div className="flex items-center">
                <Checkbox
                  id="premium-filter"
                  checked={isPremiumOnly}
                  onCheckedChange={(checked) => setIsPremiumOnly(checked as boolean)}
                />
                <label htmlFor="premium-filter" className="ml-2 text-sm">Premium Businesses Only</label>
              </div>

              <Button
                variant="outline"
                className="w-full"
                onClick={() => {
                  setSelectedCategory("");
                  setIsPremiumOnly(false);
                }}
              >
                Reset Filters
              </Button>
            </CardContent>
          </Card>

          {/* Selected Business Card */}
          {selectedBusiness && (
            <Card className="mt-4">
              <CardHeader className="pb-2">
                <CardTitle className="text-lg">{selectedBusiness.name}</CardTitle>
                <CardDescription className="flex items-center justify-between">
                  <div className="flex items-center">
                    <BiCategory className="mr-1" />
                    {selectedBusiness.category}
                  </div>
                  <div className="flex items-center text-yellow-500">
                    <BiStar className="mr-1" />
                    <span>{selectedBusiness.rating || "N/A"}</span>
                  </div>
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-3 pt-0">
                <div className="aspect-video overflow-hidden rounded-md">
                  <img
                    src={selectedBusiness.images[0].url}
                    alt={selectedBusiness.name}
                    className="w-full h-full object-cover"
                  />
                </div>

                <p className="text-sm text-muted-foreground line-clamp-3">
                  {selectedBusiness.shortDescription || selectedBusiness.description.substring(0, 100) + '...'}
                </p>

                <div className="flex justify-between items-center">
                  {selectedBusiness.isPremium && (
                    <Badge className="bg-yellow-500 hover:bg-yellow-600">Premium</Badge>
                  )}
                  <Button asChild size="sm" className="ml-auto">
                    <Link href={`/business/${selectedBusiness.id}`}>View Details</Link>
                  </Button>
                </div>
              </CardContent>
            </Card>
          )}
        </div>

        {/* Map Container */}
        <div className="md:col-span-2 lg:col-span-3">
          <div className="border rounded-md h-[600px] overflow-hidden relative">
            <LeafletMapWithNoSSR
              businesses={filteredBusinesses}
              onBusinessSelect={setSelectedBusiness}
            />
          </div>
          <p className="text-xs text-muted-foreground mt-2 text-center">
            Map data © OpenStreetMap contributors
          </p>
        </div>
      </div>
    </div>
  );
}
