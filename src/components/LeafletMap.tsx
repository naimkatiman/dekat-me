"use client";

import { useEffect } from "react";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import L from "leaflet";
import { Business } from "@/types";

// Required for Leaflet in Next.js - we need to import these in the component
import "leaflet/dist/leaflet.css";
import "leaflet/dist/images/marker-shadow.png";

// Solving the marker icon issue in Next.js
// See: https://github.com/PaulLeCam/react-leaflet/issues/808
const defaultIcon = L.icon({
  iconUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png",
  shadowUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png",
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

// Premium icon with different color
const premiumIcon = L.icon({
  iconUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png", // In a real app, use a gold/yellow marker
  shadowUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png",
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
  className: "premium-marker", // We'd use CSS to color this differently
});

// Define the center of Malaysia (approximate)
const MALAYSIA_CENTER: [number, number] = [3.139, 101.6869]; // Kuala Lumpur coordinates
const DEFAULT_ZOOM = 9;

interface LeafletMapProps {
  businesses: Business[];
  onBusinessSelect?: (business: Business) => void;
}

const LeafletMap: React.FC<LeafletMapProps> = ({ businesses, onBusinessSelect }) => {
  useEffect(() => {
    // Leaflet uses window, so we need to check if it's available
    if (typeof window !== "undefined") {
      // Fix the default icon issue by setting the default icon globally
      L.Marker.prototype.options.icon = defaultIcon;
    }
  }, []);

  return (
    <MapContainer
      center={MALAYSIA_CENTER}
      zoom={DEFAULT_ZOOM}
      style={{ height: "100%", width: "100%" }}
      scrollWheelZoom={true}
    >
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      {businesses.map((business) => (
        <Marker
          key={business.id}
          position={[business.address.latitude, business.address.longitude]}
          icon={business.isPremium ? premiumIcon : defaultIcon}
          eventHandlers={{
            click: () => {
              if (onBusinessSelect) {
                onBusinessSelect(business);
              }
            },
          }}
        >
          <Popup>
            <div className="text-center">
              <h3 className="font-bold text-sm">{business.name}</h3>
              <p className="text-xs text-gray-600">{business.category}</p>
              {business.isPremium && (
                <p className="text-xs text-yellow-600 font-medium">Premium Business</p>
              )}
              <button
                className="text-xs text-blue-600 hover:underline mt-1"
                onClick={() => {
                  if (onBusinessSelect) {
                    onBusinessSelect(business);
                  }
                }}
              >
                View Details
              </button>
            </div>
          </Popup>
        </Marker>
      ))}
    </MapContainer>
  );
};

export default LeafletMap;
