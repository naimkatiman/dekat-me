"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useAuth } from "@/contexts/AuthContext";
import { Card, CardContent } from "@/components/ui/card";
import { BiGridAlt, BiStore, BiUser, BiCategory, BiMessageSquare, BiChart, BiCog } from "react-icons/bi";

export default function AdminSidebar() {
  const pathname = usePathname();
  const { user } = useAuth();

  if (!user || user.user_metadata?.role !== 'admin') {
    return null;
  }

  const sidebarItems = [
    {
      name: "Dashboard",
      icon: <BiGridAlt className="h-5 w-5" />,
      href: "/admin",
      active: pathname === "/admin",
    },
    {
      name: "Businesses",
      icon: <BiStore className="h-5 w-5" />,
      href: "/admin/businesses",
      active: pathname.startsWith("/admin/businesses"),
    },
    {
      name: "Users",
      icon: <BiUser className="h-5 w-5" />,
      href: "/admin/users",
      active: pathname.startsWith("/admin/users"),
    },
    {
      name: "Categories",
      icon: <BiCategory className="h-5 w-5" />,
      href: "/admin/categories",
      active: pathname.startsWith("/admin/categories"),
    },
    {
      name: "Reviews",
      icon: <BiMessageSquare className="h-5 w-5" />,
      href: "/admin/reviews",
      active: pathname.startsWith("/admin/reviews"),
    },
    {
      name: "Analytics",
      icon: <BiChart className="h-5 w-5" />,
      href: "/admin/analytics",
      active: pathname.startsWith("/admin/analytics"),
    },
    {
      name: "Settings",
      icon: <BiCog className="h-5 w-5" />,
      href: "/admin/settings",
      active: pathname.startsWith("/admin/settings"),
    },
  ];

  return (
    <Card className="md:col-span-1 h-fit md:sticky md:top-20">
      <CardContent className="p-4">
        <div className="space-y-1">
          <div className="text-lg font-semibold mb-2">Admin Panel</div>
          {sidebarItems.map((item) => (
            <Link
              key={item.name}
              href={item.href}
              className={`flex items-center gap-3 px-3 py-2 rounded-md text-sm hover:bg-muted transition-colors ${
                item.active ? "bg-muted font-medium" : ""
              }`}
            >
              {item.icon}
              {item.name}
            </Link>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
