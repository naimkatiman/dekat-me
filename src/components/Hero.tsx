import { FC } from 'react';
import { Button } from './ui/button';
import Link from 'next/link';

interface HeroProps {
  title?: string;
  subtitle?: string;
  showSearchBar?: boolean;
}

export const Hero: FC<HeroProps> = ({
  title = "Find Local Businesses Near You",
  subtitle = "Discover nearby restaurants, shops, and services in your area",
  showSearchBar = true
}) => {
  return (
    <div className="relative bg-gradient-to-r from-blue-600 to-indigo-700 text-white py-20 px-4 sm:px-6 lg:px-8 rounded-lg shadow-xl overflow-hidden">
      <div className="absolute inset-0 opacity-20">
        <svg className="h-full w-full" viewBox="0 0 800 800">
          <path d="M300,220 C300,220 520,220 540,220 C740,220 640,540 520,420 C440,340 300,200 300,200" className="stroke-current text-white" strokeWidth="2" fill="none" />
          <path d="M300,320 L540,320" className="stroke-current text-white" strokeWidth="2" fill="none" />
          <path d="M300,220 C300,220 420,220 440,220 C780,220 680,540 360,420 C200,340 100,200 100,200" className="stroke-current text-white" strokeWidth="2" fill="none" />
        </svg>
      </div>
      <div className="relative max-w-3xl mx-auto text-center">
        <h1 className="text-4xl font-extrabold tracking-tight sm:text-5xl lg:text-6xl">
          {title}
        </h1>
        <p className="mt-6 text-xl lg:text-2xl max-w-xl mx-auto">
          {subtitle}
        </p>
        <div className="mt-10 max-w-md mx-auto flex flex-col sm:flex-row justify-center gap-4">
          <Link href="/categories" passHref>
            <Button size="lg" className="bg-white text-indigo-700 hover:bg-gray-100">
              Browse Categories
            </Button>
          </Link>
          <Link href="/map" passHref>
            <Button size="lg" variant="outline" className="border-white text-white hover:bg-white/10">
              View Map
            </Button>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Hero;
