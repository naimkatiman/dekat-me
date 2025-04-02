import { businesses } from "@/data/mockData";

export function generateStaticParams() {
  return businesses.map((business) => ({
    id: business.id,
  }));
}
