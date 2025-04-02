import type { Business, Category, Review, FeaturedBusiness } from "@/types";

// Mock Categories
export const categories: Category[] = [
  {
    id: "1",
    name: "Food & Beverage",
    slug: "food-beverage",
    description: "Restaurants, cafes, bakeries, and more",
    icon: "🍔",
    count: 429
  },
  {
    id: "2",
    name: "Shopping",
    slug: "shopping",
    description: "Retail stores, boutiques, and marketplaces",
    icon: "🛍️",
    count: 357
  },
  {
    id: "3",
    name: "Services",
    slug: "services",
    description: "Professional and personal services",
    icon: "🔧",
    count: 283
  },
  {
    id: "4",
    name: "Health & Beauty",
    slug: "health-beauty",
    description: "Salons, spas, gyms, and medical clinics",
    icon: "💇‍♀️",
    count: 219
  },
  {
    id: "5",
    name: "Home & Garden",
    slug: "home-garden",
    description: "Home improvement, furniture, and gardening",
    icon: "🏡",
    count: 187
  },
  {
    id: "6",
    name: "Automotive",
    slug: "automotive",
    description: "Car dealerships, repair shops, and services",
    icon: "🚗",
    count: 156
  },
  {
    id: "7",
    name: "Education",
    slug: "education",
    description: "Schools, tutoring centers, and training institutes",
    icon: "📚",
    count: 134
  },
  {
    id: "8",
    name: "Technology",
    slug: "technology",
    description: "IT services, electronics stores, and tech companies",
    icon: "💻",
    count: 128
  },
];

// Mock Featured Businesses for Homepage
export const featuredBusinesses: FeaturedBusiness[] = [
  {
    id: "1",
    name: "Kopitiams Coffee House",
    category: "Food & Beverage",
    rating: 4.7,
    reviews: 123,
    address: "Bukit Bintang, Kuala Lumpur",
    image: "https://images.unsplash.com/photo-1606812944188-2b4cc0cc6dd1?q=80&w=2070&auto=format&fit=crop",
    isPremium: true,
  },
  {
    id: "2",
    name: "Tech Solutions Malaysia",
    category: "Technology",
    rating: 4.5,
    reviews: 85,
    address: "Bangsar South, Kuala Lumpur",
    image: "https://images.unsplash.com/photo-1581092921461-7231b939a0b7?q=80&w=2070&auto=format&fit=crop",
    isPremium: true,
  },
  {
    id: "3",
    name: "Green Living Nursery",
    category: "Home & Garden",
    rating: 4.8,
    reviews: 92,
    address: "Petaling Jaya, Selangor",
    image: "https://images.unsplash.com/photo-1471193945509-9ad0617afabf?q=80&w=2070&auto=format&fit=crop",
    isPremium: false,
  },
  {
    id: "4",
    name: "Batik Boutique",
    category: "Shopping",
    rating: 4.6,
    reviews: 78,
    address: "Mont Kiara, Kuala Lumpur",
    image: "https://images.unsplash.com/photo-1620799140408-edc6dcb6d633?q=80&w=2072&auto=format&fit=crop",
    isPremium: true,
  },
];

// Detailed Business Data
export const businesses: Business[] = [
  {
    id: "1",
    name: "Kopitiams Coffee House",
    description: "A contemporary coffee shop serving traditional Malaysian coffee with modern brews. Our warm atmosphere is perfect for both quick stops and lingering visits. We source locally and roast in-house for the freshest flavors.",
    shortDescription: "Modern café with traditional Malaysian coffee",
    category: "Food & Beverage",
    subcategory: "Cafes",
    tags: ["Coffee", "Breakfast", "Malaysian", "Casual"],
    website: "https://example.com/kopitiams",
    phone: "+60123456789",
    email: "info@kopitiams.com",
    address: {
      street: "123 Jalan Bukit Bintang",
      city: "Kuala Lumpur",
      state: "Federal Territory of Kuala Lumpur",
      postalCode: "50100",
      country: "Malaysia",
      latitude: 3.1478,
      longitude: 101.7137,
      formattedAddress: "123 Jalan Bukit Bintang, Kuala Lumpur, 50100"
    },
    hours: {
      monday: { isOpen: true, open: "07:00", close: "22:00" },
      tuesday: { isOpen: true, open: "07:00", close: "22:00" },
      wednesday: { isOpen: true, open: "07:00", close: "22:00" },
      thursday: { isOpen: true, open: "07:00", close: "22:00" },
      friday: { isOpen: true, open: "07:00", close: "23:00" },
      saturday: { isOpen: true, open: "08:00", close: "23:00" },
      sunday: { isOpen: true, open: "08:00", close: "21:00" }
    },
    images: [
      {
        id: "101",
        url: "https://images.unsplash.com/photo-1606812944188-2b4cc0cc6dd1?q=80&w=2070&auto=format&fit=crop",
        alt: "Kopitiams storefront",
        isPrimary: true
      },
      {
        id: "102",
        url: "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?q=80&w=2070&auto=format&fit=crop",
        alt: "Coffee being served",
        isPrimary: false
      },
      {
        id: "103",
        url: "https://images.unsplash.com/photo-1497635491915-7bf7e3cb9b1f?q=80&w=2074&auto=format&fit=crop",
        alt: "Interior of the coffee shop",
        isPrimary: false
      }
    ],
    rating: 4.7,
    reviewCount: 123,
    isPremium: true,
    isClaimed: true,
    isVerified: true,
    features: ["Wi-Fi", "Takeaway", "Indoor Seating", "Outdoor Seating"],
    createdAt: new Date("2023-01-15"),
    updatedAt: new Date("2023-06-23")
  },
  {
    id: "2",
    name: "Tech Solutions Malaysia",
    description: "Leading tech services provider offering IT support, cloud solutions, and software development for businesses of all sizes. Our certified specialists help you navigate digital transformation with custom-tailored solutions for your needs.",
    shortDescription: "IT support and solutions",
    category: "Technology",
    subcategory: "IT Services",
    tags: ["IT Support", "Cloud Computing", "Software Development", "Business"],
    website: "https://example.com/techsolutions",
    phone: "+60323456789",
    email: "contact@techsolutionsmy.com",
    address: {
      street: "45 Jalan Kerinchi",
      city: "Kuala Lumpur",
      state: "Federal Territory of Kuala Lumpur",
      postalCode: "59200",
      country: "Malaysia",
      latitude: 3.1113,
      longitude: 101.6653,
      formattedAddress: "45 Jalan Kerinchi, Bangsar South, Kuala Lumpur, 59200"
    },
    hours: {
      monday: { isOpen: true, open: "09:00", close: "18:00" },
      tuesday: { isOpen: true, open: "09:00", close: "18:00" },
      wednesday: { isOpen: true, open: "09:00", close: "18:00" },
      thursday: { isOpen: true, open: "09:00", close: "18:00" },
      friday: { isOpen: true, open: "09:00", close: "17:00" },
      saturday: { isOpen: true, open: "10:00", close: "15:00" },
      sunday: { isOpen: false, open: "", close: "" }
    },
    images: [
      {
        id: "201",
        url: "https://images.unsplash.com/photo-1581092921461-7231b939a0b7?q=80&w=2070&auto=format&fit=crop",
        alt: "Tech Solutions office",
        isPrimary: true
      },
      {
        id: "202",
        url: "https://images.unsplash.com/photo-1573166364839-ef1b6a58487e?q=80&w=2069&auto=format&fit=crop",
        alt: "Team working on computers",
        isPrimary: false
      },
      {
        id: "203",
        url: "https://images.unsplash.com/photo-1537432376769-00f5c2f4c8d2?q=80&w=2125&auto=format&fit=crop",
        alt: "Server room",
        isPrimary: false
      }
    ],
    rating: 4.5,
    reviewCount: 85,
    isPremium: true,
    isClaimed: true,
    isVerified: true,
    features: ["Free Consultation", "Remote Support", "On-site Service", "Managed IT"],
    createdAt: new Date("2022-11-08"),
    updatedAt: new Date("2023-07-12")
  },
  {
    id: "3",
    name: "Green Living Nursery",
    description: "A sustainable plant nursery offering a wide variety of indoor and outdoor plants, gardening supplies, and landscaping services. Our experts provide guidance on plant care and sustainable gardening practices for homes and offices.",
    shortDescription: "Sustainable plant nursery and gardening center",
    category: "Home & Garden",
    subcategory: "Nurseries & Gardening",
    tags: ["Plants", "Gardening", "Sustainable", "Landscaping"],
    website: "https://example.com/greenliving",
    phone: "+60312345678",
    email: "hello@greenlivingnursery.com",
    address: {
      street: "17 Jalan SS2/55",
      city: "Petaling Jaya",
      state: "Selangor",
      postalCode: "47300",
      country: "Malaysia",
      latitude: 3.1187,
      longitude: 101.6238,
      formattedAddress: "17 Jalan SS2/55, Petaling Jaya, Selangor, 47300"
    },
    hours: {
      monday: { isOpen: true, open: "08:00", close: "19:00" },
      tuesday: { isOpen: true, open: "08:00", close: "19:00" },
      wednesday: { isOpen: true, open: "08:00", close: "19:00" },
      thursday: { isOpen: true, open: "08:00", close: "19:00" },
      friday: { isOpen: true, open: "08:00", close: "19:00" },
      saturday: { isOpen: true, open: "08:00", close: "19:00" },
      sunday: { isOpen: true, open: "09:00", close: "18:00" }
    },
    images: [
      {
        id: "301",
        url: "https://images.unsplash.com/photo-1471193945509-9ad0617afabf?q=80&w=2070&auto=format&fit=crop",
        alt: "Green Living Nursery plants display",
        isPrimary: true
      },
      {
        id: "302",
        url: "https://images.unsplash.com/photo-1459156212016-c812468e2115?q=80&w=2069&auto=format&fit=crop",
        alt: "Indoor plants section",
        isPrimary: false
      },
      {
        id: "303",
        url: "https://images.unsplash.com/photo-1584589167171-541ce45f1eea?q=80&w=2070&auto=format&fit=crop",
        alt: "Staff helping a customer",
        isPrimary: false
      }
    ],
    rating: 4.8,
    reviewCount: 92,
    isPremium: false,
    isClaimed: true,
    isVerified: true,
    features: ["Plant Care Advice", "Delivery Service", "Landscaping", "Workshops"],
    createdAt: new Date("2023-02-20"),
    updatedAt: new Date("2023-05-15")
  },
  {
    id: "4",
    name: "Batik Boutique",
    description: "Premium boutique showcasing authentic Malaysian batik designs in contemporary clothing, accessories, and home decor. Our artisanal products are handcrafted by local artisans, supporting traditional craftsmanship with modern aesthetics.",
    shortDescription: "Authentic Malaysian batik fashion and decor",
    category: "Shopping",
    subcategory: "Clothing & Accessories",
    tags: ["Batik", "Fashion", "Handicrafts", "Malaysian"],
    website: "https://example.com/batikboutique",
    phone: "+60378901234",
    email: "shop@batikboutique.com",
    address: {
      street: "28 Jalan Kiara",
      city: "Kuala Lumpur",
      state: "Federal Territory of Kuala Lumpur",
      postalCode: "50480",
      country: "Malaysia",
      latitude: 3.1636,
      longitude: 101.6562,
      formattedAddress: "28 Jalan Kiara, Mont Kiara, Kuala Lumpur, 50480"
    },
    hours: {
      monday: { isOpen: true, open: "10:00", close: "20:00" },
      tuesday: { isOpen: true, open: "10:00", close: "20:00" },
      wednesday: { isOpen: true, open: "10:00", close: "20:00" },
      thursday: { isOpen: true, open: "10:00", close: "20:00" },
      friday: { isOpen: true, open: "10:00", close: "22:00" },
      saturday: { isOpen: true, open: "10:00", close: "22:00" },
      sunday: { isOpen: true, open: "11:00", close: "19:00" }
    },
    images: [
      {
        id: "401",
        url: "https://images.unsplash.com/photo-1620799140408-edc6dcb6d633?q=80&w=2072&auto=format&fit=crop",
        alt: "Batik Boutique storefront",
        isPrimary: true
      },
      {
        id: "402",
        url: "https://images.unsplash.com/photo-1612649487535-e8b8a7b936b5?q=80&w=2071&auto=format&fit=crop",
        alt: "Batik clothing display",
        isPrimary: false
      },
      {
        id: "403",
        url: "https://images.unsplash.com/photo-1583394838336-acd977736f90?q=80&w=2068&auto=format&fit=crop",
        alt: "Handcrafted accessories",
        isPrimary: false
      }
    ],
    rating: 4.6,
    reviewCount: 78,
    isPremium: true,
    isClaimed: true,
    isVerified: true,
    features: ["Custom Orders", "Gift Wrapping", "Cultural Workshops", "International Shipping"],
    createdAt: new Date("2022-09-10"),
    updatedAt: new Date("2023-08-05")
  }
];

// Mock Reviews
export const reviews: Record<string, Review[]> = {
  "1": [
    {
      id: "r101",
      businessId: "1",
      userId: "u1",
      userName: "Ahmad Rahman",
      userAvatar: "https://randomuser.me/api/portraits/men/32.jpg",
      rating: 5,
      comment: "Best kopi in KL! The atmosphere is cozy and the staff are very friendly. I love coming here to work or meet friends. Their traditional kopi is excellent and they have great pastries too.",
      likes: 12,
      createdAt: new Date("2023-05-10"),
      updatedAt: new Date("2023-05-10")
    },
    {
      id: "r102",
      businessId: "1",
      userId: "u2",
      userName: "Siti Aminah",
      userAvatar: "https://randomuser.me/api/portraits/women/44.jpg",
      rating: 4,
      comment: "Good coffee and nice environment. Gets a bit crowded during lunch hours but service remains quick. I particularly like their specialty brews and the free wifi is reliable.",
      likes: 8,
      createdAt: new Date("2023-04-22"),
      updatedAt: new Date("2023-04-22")
    },
    {
      id: "r103",
      businessId: "1",
      userId: "u3",
      userName: "David Tan",
      userAvatar: "https://randomuser.me/api/portraits/men/62.jpg",
      rating: 5,
      comment: "This place has become my regular spot! The coffee is consistently excellent and the staff remember regular customers. The outdoor seating area is perfect on cool evenings.",
      likes: 15,
      createdAt: new Date("2023-06-05"),
      updatedAt: new Date("2023-06-05")
    }
  ],
  "2": [
    {
      id: "r201",
      businessId: "2",
      userId: "u4",
      userName: "Mei Ling",
      userAvatar: "https://randomuser.me/api/portraits/women/17.jpg",
      rating: 5,
      comment: "Excellent IT support for our business. Their team was quick to respond when our systems went down and provided continuous support until everything was resolved. Highly professional.",
      likes: 7,
      createdAt: new Date("2023-03-15"),
      updatedAt: new Date("2023-03-15")
    },
    {
      id: "r202",
      businessId: "2",
      userId: "u5",
      userName: "Raj Kumar",
      userAvatar: "https://randomuser.me/api/portraits/men/45.jpg",
      rating: 4,
      comment: "Good service but pricing is a bit on the higher side. They helped us migrate to cloud services smoothly. Their expertise is definitely worth it for critical IT infrastructure.",
      likes: 3,
      createdAt: new Date("2023-05-28"),
      updatedAt: new Date("2023-05-28")
    }
  ],
  "3": [
    {
      id: "r301",
      businessId: "3",
      userId: "u6",
      userName: "Fatimah Zahra",
      userAvatar: "https://randomuser.me/api/portraits/women/22.jpg",
      rating: 5,
      comment: "Amazing selection of plants! The staff are very knowledgeable and helped me choose plants that would thrive in my apartment. They even followed up to check how my plants were doing.",
      likes: 21,
      createdAt: new Date("2023-04-12"),
      updatedAt: new Date("2023-04-12")
    },
    {
      id: "r302",
      businessId: "3",
      userId: "u7",
      userName: "James Wong",
      userAvatar: "https://randomuser.me/api/portraits/men/73.jpg",
      rating: 4,
      comment: "Great place for garden supplies. Prices are reasonable and the quality of plants is excellent. I attended one of their workshops on sustainable gardening which was very informative.",
      likes: 9,
      createdAt: new Date("2023-05-02"),
      updatedAt: new Date("2023-05-02")
    },
    {
      id: "r303",
      businessId: "3",
      userId: "u8",
      userName: "Nurul Izzah",
      userAvatar: "https://randomuser.me/api/portraits/women/89.jpg",
      rating: 5,
      comment: "I love this nursery! They have plants you can't find elsewhere and their gardening advice has been spot on. The delivery service is convenient and they take care with packaging.",
      likes: 14,
      createdAt: new Date("2023-06-18"),
      updatedAt: new Date("2023-06-18")
    }
  ],
  "4": [
    {
      id: "r401",
      businessId: "4",
      userId: "u9",
      userName: "Aisha Lim",
      userAvatar: "https://randomuser.me/api/portraits/women/33.jpg",
      rating: 5,
      comment: "Absolutely beautiful batik products! I bought several pieces as gifts and everyone loved them. The quality is exceptional and you can tell they're made with care. Will definitely shop here again.",
      likes: 18,
      createdAt: new Date("2023-03-25"),
      updatedAt: new Date("2023-03-25")
    },
    {
      id: "r402",
      businessId: "4",
      userId: "u10",
      userName: "Hassan Abdullah",
      userAvatar: "https://randomuser.me/api/portraits/men/55.jpg",
      rating: 4,
      comment: "Great selection of authentic Malaysian batik. The prices reflect the quality and craftsmanship. I appreciated learning about the process from the staff who are clearly passionate about preserving this art form.",
      likes: 6,
      createdAt: new Date("2023-05-15"),
      updatedAt: new Date("2023-05-15")
    }
  ]
};
