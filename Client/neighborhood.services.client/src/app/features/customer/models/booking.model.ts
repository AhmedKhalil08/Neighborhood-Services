// Mirrors the backend enums (serialized as strings via JsonStringEnumConverter)
export type BookingType = 'Direct' | 'Bidding' | 'Recurring';
// "Quoted" = Direct flow only: tech has set FinalPrice + DurationMinutes, customer must accept / reject.
export type BookingStatus = 'Pending' | 'Quoted' | 'Confirmed' | 'Completed' | 'Cancelled' | 'Disputed';

// Body for POST /api/bookings (Direct booking). Customer is resolved from the token server-side.
export interface CreateBooking {
  technicianId: number;
  problemTypeId: number;
  description: string;
  address: string;
  latitude: number;
  longitude: number;
  region?: string | null;
  scheduledAt: string;   // "yyyy-MM-ddTHH:mm:ss" (must be in the future)
  promoCodeId?: number | null;
  beforeImageUrl?: string | null;  // optional photo of the problem, shown to the tech before quoting
}

// Mirrors BookingSummaryDto (used by GET /api/bookings/recurring/{id} and admin lists).
export interface BookingSummary {
  id: number;
  bookingType: BookingType;
  description: string;
  address: string;
  scheduledAt: string;    // ISO date string from the API
  estimatedPrice: number;
  status: BookingStatus;
  createdAt: string;      // ISO date string
}

// Mirrors MyBookingSummaryDto — only returned by GET /api/bookings/mine.
// Includes the quoted FinalPrice + DurationMinutes (set by the technician on the
// Pending -> Quoted transition) and the (TechnicianId, ProblemTypeId) so the UI
// can fetch the tech's pricing range for that problem type.
export interface MyBookingSummary {
  id: number;
  bookingType: BookingType;
  description: string;
  address: string;
  scheduledAt: string;
  estimatedPrice: number;
  finalPrice: number;
  durationMinutes?: number | null;
  status: BookingStatus;
  clientConfirmed: boolean;
  createdAt: string;
  technicianId: number;
  problemTypeId: number;
  latitude: number;
  longitude: number;
}

// Mirrors TechnicianPricingRangeDto (GET /api/bookings/tech-pricing-range)
export interface TechnicianPricingRange {
  technicianId: number;
  problemTypeId: number;
  minPrice: number;
  maxPrice: number;
}

// Mirrors BookingDetailsDto (GET /api/bookings/{id})
export interface BookingDetails {
  id: number;
  bookingType: BookingType;
  description: string;
  address: string;
  scheduledAt: string;
  estimatedPrice: number;
  finalPrice: number;
  status: BookingStatus;
  clientConfirmed: boolean;
  cancellationReason?: string | null;
  createdAt: string;
  customerId: number;
  technicianId: number;
  problemTypeId: number;
  offerId?: number | null;
  serviceRequestId?: number | null;
  latitude: number;
  longitude: number;
}

// Mirrors BookingImageDto (GET /api/bookings/{id}/images)
export type BookingImageType = 'Before' | 'After';

export interface BookingImage {
  id: number;
  bookingId: number;
  imageUrl: string;
  type: BookingImageType;
  uploadedBy: string;
  uploadedAt: string;
}
