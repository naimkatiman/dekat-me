import { notFound } from "next/navigation";
import Link from "next/link";
import { BiPhone, BiGlobe, BiEnvelope, BiMap, BiStar, BiTime, BiCheck } from "react-icons/bi";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { businesses, reviews as allReviews } from "@/data/mockData";
import { formatDate } from "@/lib/utils";
import { Business, type BusinessHours as BusinessHoursType, type DayHours, type Review } from "@/types";

// Generate static params for all business IDs
export function generateStaticParams() {
  return businesses.map((business) => ({
    id: business.id,
  }));
}

// Helper function to find a business by ID
function getBusinessById(id: string) {
  return businesses.find((business) => business.id === id);
}

// Helper function to get reviews for a business
function getReviewsForBusiness(id: string) {
  return allReviews[id] || [];
}

// Helper function to format day name
function formatDayName(day: string) {
  return day.charAt(0).toUpperCase() + day.slice(1);
}

// Component for business hours display
function BusinessHours({ hours }: { hours: BusinessHoursType }) {
  const daysOfWeek = ["monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"];

  return (
    <div className="space-y-2">
      {daysOfWeek.map((day) => {
        const dayHours = hours[day as keyof BusinessHoursType] as DayHours | undefined;
        return (
          <div key={day} className="flex justify-between items-center text-sm">
            <span className="font-medium">{formatDayName(day)}</span>
            {dayHours?.isOpen ? (
              <span>{dayHours.open} - {dayHours.close}</span>
            ) : (
              <span className="text-muted-foreground">Closed</span>
            )}
          </div>
        );
      })}
    </div>
  );
}

// Component for reviews
function ReviewItem({ review }: { review: Review }) {
  return (
    <div className="border rounded-lg p-4 mb-4">
      <div className="flex items-start justify-between">
        <div className="flex items-center gap-3">
          <img
            src={review.userAvatar || "https://via.placeholder.com/40"}
            alt={review.userName}
            className="w-10 h-10 rounded-full object-cover"
          />
          <div>
            <h4 className="font-medium">{review.userName}</h4>
            <p className="text-xs text-muted-foreground">{formatDate(review.createdAt)}</p>
          </div>
        </div>
        <div className="flex items-center bg-yellow-50 text-yellow-600 px-2 py-1 rounded text-sm">
          <BiStar className="mr-1" />
          {review.rating}
        </div>
      </div>
      <p className="mt-3 text-sm">{review.comment}</p>
    </div>
  );
}

export default function BusinessDetailsPage({ params }: { params: { id: string } }) {
  const business = getBusinessById(params.id);

  if (!business) {
    notFound();
  }

  const reviews = getReviewsForBusiness(business.id);

  return (
    <div className="container px-4 mx-auto py-8">
      {/* Rest of the component remains the same */}
      {/* Business Header */}
      <div className="flex flex-col md:flex-row justify-between items-start gap-4 mb-6">
        <div>
          <div className="flex items-center gap-2 mb-2">
            <Badge variant="outline" className="text-xs font-normal">
              {business.category}
            </Badge>
            {business.isPremium && (
              <Badge className="bg-yellow-500 hover:bg-yellow-600">Premium</Badge>
            )}
          </div>
          <h1 className="text-3xl font-bold">{business.name}</h1>
          <p className="text-muted-foreground">{business.shortDescription}</p>

          <div className="flex items-center mt-2 gap-4">
            <div className="flex items-center">
              <BiStar className="text-yellow-500 mr-1" />
              <span className="font-medium">{business.rating}</span>
              <span className="text-muted-foreground ml-1">({business.reviewCount} reviews)</span>
            </div>

            {business.isClaimed && (
              <div className="flex items-center text-sm text-blue-600">
                <BiCheck className="mr-1" />
                Claimed
              </div>
            )}

            {business.isVerified && (
              <div className="flex items-center text-sm text-green-600">
                <BiCheck className="mr-1" />
                Verified
              </div>
            )}
          </div>
        </div>

        <div className="flex gap-2">
          <Button asChild variant="outline">
            <Link href={`https://maps.google.com/?q=${business.address.latitude},${business.address.longitude}`} target="_blank">Get Directions</Link>
          </Button>
          <Button asChild>
            <Link href={`tel:${business.phone}`}>Call Now</Link>
          </Button>
        </div>
      </div>

      {/* Gallery */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
        {business.images.map((image, index) => (
          <div
            key={image.id}
            className={`rounded-lg overflow-hidden ${index === 0 ? 'md:col-span-2 md:row-span-2' : ''}`}
          >
            <img
              src={image.url}
              alt={image.alt || business.name}
              className="w-full h-full object-cover aspect-video"
            />
          </div>
        ))}
      </div>

      {/* Tabs section */}
      <Tabs defaultValue="details" className="mb-8">
        <TabsList className="mb-4">
          <TabsTrigger value="details">Details</TabsTrigger>
          <TabsTrigger value="reviews">Reviews ({reviews.length})</TabsTrigger>
        </TabsList>

        <TabsContent value="details" className="space-y-8">
          {/* Details Content */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="md:col-span-2">
              <h2 className="text-xl font-bold mb-4">About {business.name}</h2>
              <p className="mb-6">{business.description}</p>

              {business.features && business.features.length > 0 && (
                <div className="mb-6">
                  <h3 className="font-semibold mb-2">Features</h3>
                  <div className="flex flex-wrap gap-2">
                    {business.features.map((feature) => (
                      <Badge variant="secondary" key={feature}>
                        {feature}
                      </Badge>
                    ))}
                  </div>
                </div>
              )}

              {business.tags && business.tags.length > 0 && (
                <div>
                  <h3 className="font-semibold mb-2">Tags</h3>
                  <div className="flex flex-wrap gap-2">
                    {business.tags.map((tag) => (
                      <Badge variant="outline" key={tag}>
                        {tag}
                      </Badge>
                    ))}
                  </div>
                </div>
              )}
            </div>

            <div className="space-y-6">
              <div>
                <h3 className="font-semibold mb-2">Contact Information</h3>
                <div className="space-y-2">
                  {business.phone && (
                    <p className="flex items-center text-sm">
                      <BiPhone className="mr-2 text-muted-foreground" />
                      <a href={`tel:${business.phone}`} className="hover:underline">
                        {business.phone}
                      </a>
                    </p>
                  )}

                  {business.email && (
                    <p className="flex items-center text-sm">
                      <BiEnvelope className="mr-2 text-muted-foreground" />
                      <a href={`mailto:${business.email}`} className="hover:underline">
                        {business.email}
                      </a>
                    </p>
                  )}

                  {business.website && (
                    <p className="flex items-center text-sm">
                      <BiGlobe className="mr-2 text-muted-foreground" />
                      <a href={business.website} target="_blank" className="hover:underline" rel="noreferrer">
                        {business.website.replace(/(^\w+:|^)\/\//, '')}
                      </a>
                    </p>
                  )}
                </div>
              </div>

              <div>
                <h3 className="font-semibold mb-2">Location</h3>
                <p className="flex items-start text-sm mb-2">
                  <BiMap className="mr-2 text-muted-foreground shrink-0 mt-1" />
                  <span>
                    {business.address.street}, {business.address.city}, {business.address.state} {business.address.postalCode}
                  </span>
                </p>
                <div className="rounded-lg overflow-hidden border h-40 bg-muted">
                  {/* We would normally have a map here */}
                  <div className="w-full h-full flex items-center justify-center text-center p-4 text-muted-foreground">
                    <p>Map would be displayed here with coordinates:<br />
                    {business.address.latitude}, {business.address.longitude}</p>
                  </div>
                </div>
              </div>

              <div>
                <h3 className="font-semibold mb-2 flex items-center">
                  <BiTime className="mr-2" /> Business Hours
                </h3>
                {business.hours && <BusinessHours hours={business.hours} />}
              </div>
            </div>
          </div>
        </TabsContent>

        <TabsContent value="reviews">
          {/* Reviews Content */}
          <div>
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-xl font-bold">Customer Reviews</h2>
              <Button asChild variant="outline">
                <Link href={`/business/${business.id}/review`}>Write a Review</Link>
              </Button>
            </div>

            {reviews.length > 0 ? (
              <div className="space-y-4">
                {reviews.map((review) => (
                  <ReviewItem key={review.id} review={review} />
                ))}
              </div>
            ) : (
              <div className="text-center py-8 text-muted-foreground">
                <p>No reviews yet. Be the first to write a review!</p>
              </div>
            )}
          </div>
        </TabsContent>
      </Tabs>

      {/* Related Businesses Section - we'd normally have this */}
      <div className="mt-12">
        <h2 className="text-xl font-bold mb-6">Similar Businesses</h2>
        <p className="text-muted-foreground">Related businesses in {business.category} would be displayed here.</p>
      </div>
    </div>
  );
}
