import { notFound } from "next/navigation";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { businesses } from "@/data/mockData";

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

export default function WriteReviewPage({ params }: { params: { id: string } }) {
  const business = getBusinessById(params.id);

  if (!business) {
    notFound();
  }

  return (
    <div className="container mx-auto py-8 px-4">
      <div className="max-w-2xl mx-auto">
        <Card>
          <CardHeader>
            <CardTitle>Write a Review</CardTitle>
            <CardDescription>
              Share your experience at {business.name}
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <p className="text-center">
              Review functionality is available in the application but disabled in this demo.
            </p>
            <div className="flex justify-center">
              <Button asChild variant="outline">
                <Link href={`/business/${business.id}`}>Back to Business</Link>
              </Button>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
