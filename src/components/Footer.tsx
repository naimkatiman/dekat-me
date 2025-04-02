import Link from "next/link";

export function Footer() {
  return (
    <footer className="bg-background border-t py-8 px-4 md:px-6">
      <div className="container mx-auto">
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-8">
          <div>
            <h3 className="text-lg font-bold mb-4">dekat.me</h3>
            <p className="text-muted-foreground mb-4">
              A modern business directory for Malaysian businesses - discover local businesses with geolocation search.
            </p>
          </div>
          <div>
            <h3 className="text-lg font-bold mb-4">Explore</h3>
            <ul className="space-y-2">
              <li>
                <Link href="/categories" className="text-muted-foreground hover:text-foreground transition">
                  Categories
                </Link>
              </li>
              <li>
                <Link href="/popular" className="text-muted-foreground hover:text-foreground transition">
                  Popular Businesses
                </Link>
              </li>
              <li>
                <Link href="/new" className="text-muted-foreground hover:text-foreground transition">
                  New Listings
                </Link>
              </li>
              <li>
                <Link href="/map" className="text-muted-foreground hover:text-foreground transition">
                  Map View
                </Link>
              </li>
            </ul>
          </div>
          <div>
            <h3 className="text-lg font-bold mb-4">For Businesses</h3>
            <ul className="space-y-2">
              <li>
                <Link href="/submit-business" className="text-muted-foreground hover:text-foreground transition">
                  Add Your Business
                </Link>
              </li>
              <li>
                <Link href="/claim-business" className="text-muted-foreground hover:text-foreground transition">
                  Claim Your Business
                </Link>
              </li>
              <li>
                <Link href="/subscription" className="text-muted-foreground hover:text-foreground transition">
                  Premium Subscription
                </Link>
              </li>
              <li>
                <Link href="/spotlight" className="text-muted-foreground hover:text-foreground transition">
                  Spotlight Tokens
                </Link>
              </li>
            </ul>
          </div>
          <div>
            <h3 className="text-lg font-bold mb-4">Company</h3>
            <ul className="space-y-2">
              <li>
                <Link href="/about" className="text-muted-foreground hover:text-foreground transition">
                  About Us
                </Link>
              </li>
              <li>
                <Link href="/contact" className="text-muted-foreground hover:text-foreground transition">
                  Contact
                </Link>
              </li>
              <li>
                <Link href="/privacy" className="text-muted-foreground hover:text-foreground transition">
                  Privacy Policy
                </Link>
              </li>
              <li>
                <Link href="/terms" className="text-muted-foreground hover:text-foreground transition">
                  Terms of Service
                </Link>
              </li>
            </ul>
          </div>
        </div>
        <div className="border-t mt-8 pt-8 text-center text-muted-foreground">
          <p>© {new Date().getFullYear()} dekat.me. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
}
