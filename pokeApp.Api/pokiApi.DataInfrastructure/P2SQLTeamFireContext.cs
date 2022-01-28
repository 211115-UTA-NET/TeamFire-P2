using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace pokiApi.DataInfrastructure
{
    public partial class P2SQLTeamFireContext : DbContext
    {
        public P2SQLTeamFireContext()
        {
        }

        public P2SQLTeamFireContext(DbContextOptions<P2SQLTeamFireContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<CompletedTrade> CompletedTrades { get; set; } = null!;
        public virtual DbSet<Dex> Dices { get; set; } = null!;
        public virtual DbSet<TradeDetail> TradeDetails { get; set; } = null!;
        public virtual DbSet<TradeRequest> TradeRequests { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Cards", "poke");

                entity.Property(e => e.CardId).HasColumnName("cardID");

                entity.Property(e => e.PokeId).HasColumnName("pokeID");

                entity.Property(e => e.Trading)
                    .HasColumnName("trading")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Poke)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.PokeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Poke_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_ID");
            });

            modelBuilder.Entity<CompletedTrade>(entity =>
            {
                entity.HasKey(e => e.TradeId)
                    .HasName("PK__Complete__F7D149FDC755D822");

                entity.ToTable("CompletedTrades", "poke");

                entity.Property(e => e.TradeId).HasColumnName("tradeID");

                entity.Property(e => e.OfferedBy).HasColumnName("offeredBy");

                entity.Property(e => e.RedeemedBy).HasColumnName("redeemedBy");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.HasOne(d => d.OfferedByNavigation)
                    .WithMany(p => p.CompletedTradeOfferedByNavigations)
                    .HasForeignKey(d => d.OfferedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_offeredBy_User_ID");

                entity.HasOne(d => d.RedeemedByNavigation)
                    .WithMany(p => p.CompletedTradeRedeemedByNavigations)
                    .HasForeignKey(d => d.RedeemedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_redeemedBy_User_ID");
            });

            modelBuilder.Entity<Dex>(entity =>
            {
                entity.HasKey(e => e.PokeId)
                    .HasName("PK__Dex__86FB3208B8A8C8E0");

                entity.ToTable("Dex", "poke");

                entity.Property(e => e.PokeId).HasColumnName("pokeID");

                entity.Property(e => e.Pokemon)
                    .HasMaxLength(255)
                    .HasColumnName("pokemon");
            });

            modelBuilder.Entity<TradeDetail>(entity =>
            {
                entity.ToTable("TradeDetail", "poke");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CardId).HasColumnName("cardID");

                entity.Property(e => e.TradeId).HasColumnName("tradeID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.TradeDetails)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_card_ID");

                entity.HasOne(d => d.Trade)
                    .WithMany(p => p.TradeDetails)
                    .HasForeignKey(d => d.TradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trade_ID");
            });

            modelBuilder.Entity<TradeRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK__TradeReq__E3C5DE51F217DB38");

                entity.ToTable("TradeRequest", "poke");

                entity.Property(e => e.RequestId).HasColumnName("requestID");

                entity.Property(e => e.CardId).HasColumnName("cardID");

                entity.Property(e => e.OfferCardId).HasColumnName("offerCardID");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasColumnName("status");

                entity.Property(e => e.TargetUserId).HasColumnName("targetUserID");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.TradeRequestCards)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TR_FK_card_ID");

                entity.HasOne(d => d.OfferCard)
                    .WithMany(p => p.TradeRequestOfferCards)
                    .HasForeignKey(d => d.OfferCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TR_FK_offer_card_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TradeRequests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TR_FK_user_ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "poke");

                entity.HasIndex(e => e.UserName, "UQ__Users__66DCF95C4C380F16")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
